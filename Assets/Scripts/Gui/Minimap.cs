using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{
	public UITexture mapTexture;

	private float m_extent = 1024;
	public float Extent
	{
		get { return m_extent; }
		set { m_extent = value; Layout = true; }
	}

	public bool Layout { get; set; }

	public void Setup()
	{
		mapTexture.mainTexture = BattleScene.Instance.MapNav.transform.parent.GetComponentInChildren<MapTexture>().texture;
		mapTexture.gameObject.SetActive(mapTexture.mainTexture != null);

		if (MainRole.Instance != null)
			MainRole.Instance.entity.PositionChanged += OnMainRolePositionChanged;
		OnMainRolePositionChanged(MainRole.Instance.entity);
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
		if (Layout && mapTexture.gameObject.activeSelf)
		{
			Layout = false;
			var mapNav = BattleScene.Instance.MapNav;
			var size = new Vector2(mapNav.gridWidth * mapNav.gridXNum, mapNav.gridHeight * mapNav.gridZNum);

			var pos = MainRole.Instance.entity.Position;

			var material = mapTexture.material;
			material.SetFloat("_Cutoff", mapTexture.mainTexture == null ? 0 : 0.1f);
			material.mainTextureOffset = new Vector2(
				Mathf.Clamp((pos.x - Extent / mapTexture.mainTexture.width * 0.5f) / size.x, 0, 1 - Extent / mapTexture.mainTexture.width),
				Mathf.Clamp((pos.y - Extent / mapTexture.mainTexture.height * 0.5f) / size.y, 0, 1 - Extent / mapTexture.mainTexture.height));
			material.mainTextureScale = new Vector2(
				Extent / mapTexture.mainTexture.width,
				Extent / mapTexture.mainTexture.height);

			// force update
			if (mapTexture.panel != null)
			{
				mapTexture.panel.RemoveWidget(mapTexture);
				mapTexture.panel = null;
			}
		}
	}
}
