using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{
	public UITexture map;
	private float m_scale = 1;
	public float Scale
	{
		get { return m_scale; }
		set { m_scale = value; Layout(); }
	}

	void Start()
	{
		MainRole.Instance.entity.PositionChanged += Layout;
	}

	void OnDestroy()
	{
		MainRole.Instance.entity.PositionChanged -= Layout;
	}

	private void Layout(Entity obj = null)
	{
		var pos = MainRole.Instance.entity.Position;
		var map = BattleScene.Instance.MapNav;
		var size = new Vector2(map.gridWidth * map.gridXNum, map.gridHeight * map.gridZNum);
	}
}
