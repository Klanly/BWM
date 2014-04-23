using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{
	public UITexture uiMapTexture;
	public UISprite uiFlagMainRole;
	public UISprite uiFlagRole;
	public UISprite[] uiFlagNpc;

	private float m_extent = 1024;
	public float Extent
	{
		get { return m_extent; }
		set { m_extent = value; Layout = true; }
	}

	public bool Layout { get; set; }

	private Material material;

	public void Setup()
	{
		uiMapTexture.mainTexture = BattleScene.Instance.MapNav.transform.parent.GetComponentInChildren<MapTexture>().texture;
		uiMapTexture.gameObject.SetActive(uiMapTexture.mainTexture != null);

		if (MainRole.Instance != null)
			MainRole.Instance.entity.PositionChanged += OnMainRolePositionChanged;
		OnMainRolePositionChanged(MainRole.Instance.entity);
	}

	void Start()
	{
		// 对material clone一份，防止运行时的修改影响到源文件
		material = (Material)GameObject.Instantiate(uiMapTexture.material);
		uiMapTexture.material = material;
	}

	void OnDestroy()
	{
		if (MainRole.Instance != null)
			MainRole.Instance.entity.PositionChanged -= OnMainRolePositionChanged;
	}

	void OnMainRolePositionChanged(Entity sender)
	{
		Layout = true;
	}

	void Update()
	{
		if (Layout && uiMapTexture.gameObject.activeSelf)
		{
			Layout = false;
			var mapNav = BattleScene.Instance.MapNav;
			var size = new Vector2(mapNav.gridWidth * mapNav.gridXNum, mapNav.gridHeight * mapNav.gridZNum);
			Extent = Mathf.Min(Extent, uiMapTexture.mainTexture.width, uiMapTexture.mainTexture.height);
			var relativeExtent = new Vector2(Extent / (float)uiMapTexture.mainTexture.width, Extent / (float)uiMapTexture.mainTexture.height);

			var relativePos = new Vector2(MainRole.Instance.entity.Position.x / size.x, MainRole.Instance.entity.Position.z / size.y);

			// 地图位置更新
			material.mainTextureOffset = new Vector2(
				Mathf.Clamp(relativePos.x - 0.5f * relativeExtent.x, 0.0f, 1.0f - relativeExtent.x),
				Mathf.Clamp(relativePos.y - 0.5f * relativeExtent.y, 0.0f, 1.0f - relativeExtent.y));
			material.mainTextureScale = relativeExtent;

			// force update
			if (uiMapTexture.panel != null)
			{
				uiMapTexture.panel.RemoveWidget(uiMapTexture);
				uiMapTexture.panel = null;
			}

			// 主角图标位置更新
			LayoutFlag(MainRole.Instance.entity, uiFlagMainRole);
		}

		// 主角图标显隐
		uiFlagMainRole.gameObject.SetActive(MainRole.Instance != null);
		// 主角图标旋转
		if (MainRole.Instance != null)
			uiFlagMainRole.transform.localRotation = Quaternion.Euler(0, 180, MainRole.Instance.transform.localRotation.eulerAngles.y);
	}

	void LayoutFlag(Entity entity, UISprite flag)
	{
		var mapNav = BattleScene.Instance.MapNav;
		var size = new Vector2(mapNav.gridWidth * mapNav.gridXNum, mapNav.gridHeight * mapNav.gridZNum);
		var relativeExtent = new Vector2(Extent / (float)uiMapTexture.mainTexture.width, Extent / (float)uiMapTexture.mainTexture.height);

		var relativePos = new Vector2(entity.Position.x / size.x, entity.Position.z / size.y);
		var p = relativePos - material.mainTextureOffset;
		p.x /= relativeExtent.x;
		p.y /= relativeExtent.y;
		uiFlagMainRole.leftAnchor.Set(p.x, -uiFlagMainRole.width * 0.5f);
		uiFlagMainRole.rightAnchor.Set(p.x, uiFlagMainRole.width * 0.5f);
		uiFlagMainRole.topAnchor.Set(p.y, uiFlagMainRole.height * 0.5f);
		uiFlagMainRole.bottomAnchor.Set(p.y, -uiFlagMainRole.height * 0.5f);
		uiFlagMainRole.UpdateAnchors();
	}


	UISprite GetNpcFlag(Npc npc)
	{
		var index = (int)npc.TableInfo.BaseType;
		if(index >= 0 && index < uiFlagNpc.Length)
			return uiFlagNpc[index];
		return null;
	}
}
