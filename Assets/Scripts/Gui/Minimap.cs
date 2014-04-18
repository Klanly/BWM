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

			var pos = MainRole.Instance.entity.Position;

			material.SetFloat("_Cutoff", uiMapTexture.mainTexture == null ? 0 : 0.1f);
			material.mainTextureOffset = new Vector2(
				Mathf.Clamp((pos.x - Extent / uiMapTexture.mainTexture.width * 0.5f) / size.x, 0, 1 - Extent / uiMapTexture.mainTexture.width),
				Mathf.Clamp((pos.y - Extent / uiMapTexture.mainTexture.height * 0.5f) / size.y, 0, 1 - Extent / uiMapTexture.mainTexture.height));
			material.mainTextureScale = new Vector2(
				Extent / uiMapTexture.mainTexture.width,
				Extent / uiMapTexture.mainTexture.height);

			// force update
			if (uiMapTexture.panel != null)
			{
				uiMapTexture.panel.RemoveWidget(uiMapTexture);
				uiMapTexture.panel = null;
			}
		}
	}
}
