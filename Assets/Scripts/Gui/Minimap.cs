using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{
	public UITexture uiMapTexture;
	public GameObject uiFlagMainRole;

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

			var pos = MainRole.Instance.entity.Position;

			var ex = Extent / (float)uiMapTexture.mainTexture.width;
			var ey = Extent / (float)uiMapTexture.mainTexture.height;

			// 地图位置更新
			material.mainTextureOffset = new Vector2(
				Mathf.Clamp(pos.x / size.x - 0.5f * ex, 0.0f, 1.0f - ex),
				Mathf.Clamp(pos.z / size.y - 0.5f * ey, 0.0f, 1.0f - ey));
			material.mainTextureScale = new Vector2(
				Extent / (float)uiMapTexture.mainTexture.width,
				Extent / (float)uiMapTexture.mainTexture.height);

			// force update
			if (uiMapTexture.panel != null)
			{
				uiMapTexture.panel.RemoveWidget(uiMapTexture);
				uiMapTexture.panel = null;
			}

			// 主角图标位置更新
			uiFlagMainRole.transform.localPosition = uiMapTexture.transform.localPosition;

			//var p = new Vector2(pos.x / size.x, pos.z / size.y);
			//Debug.Log(string.Format("{0} - {1} = {2}", p.Dump(), material.mainTextureOffset.Dump(), (p - material.mainTextureOffset).Dump()));
		}

		// 主角图标显隐
		uiFlagMainRole.SetActive(MainRole.Instance != null);
		// 主角图标旋转
		if (MainRole.Instance != null)
			uiFlagMainRole.transform.localRotation = Quaternion.Euler(0, 180, MainRole.Instance.transform.localRotation.eulerAngles.y);
	}
}
