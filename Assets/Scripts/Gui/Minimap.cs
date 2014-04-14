using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{
	public UITexture mapTexture;

	private float m_extent = 512;
	public float Extent
	{
		get { return m_extent; }
		set { m_extent = value; Layout(); }
	}

	public void Setup()
	{
		if (MainRole.Instance != null)
			MainRole.Instance.entity.PositionChanged += Layout;
	}

	void Start()
	{
		
	}

	void OnDestroy()
	{
		if (MainRole.Instance != null)
			MainRole.Instance.entity.PositionChanged -= Layout;
	}

	private void Layout(Entity obj = null)
	{
		if (mapTexture.mainTexture == null)
			mapTexture.mainTexture = BattleScene.Instance.MapNav.transform.parent.GetComponentInChildren<MapTexture>().texture;
		var mapNav = BattleScene.Instance.MapNav;
		var size = new Vector2(mapNav.gridWidth * mapNav.gridXNum, mapNav.gridHeight * mapNav.gridZNum);

		var pos = MainRole.Instance.entity.Position;

		//mapTexture.material.mainTextureOffset = new Vector2(
		//	(pos.x - Extent * 0.5f) / size.x,
		//	(pos.y - Extent * 0.5f) / size.y);
		//mapTexture.material.mainTextureScale = new Vector2(
		//	size.x / Extent,
		//	size.y / Extent);
		//mapTexture.SetDirty();
	}
}
