using UnityEngine;
using System.Collections;
using System;
using Cmd;


public class Entity : MonoBehaviour
{

	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }

	/// <summary>
	/// 移动速度
	/// </summary>
	public float Speed = 5.0f;

	/// <summary>
	/// 角色移动事件
	/// </summary>
	public event Action<Entity> PositionChanged;

	/// <summary>
	/// 角色世界坐标位置
	/// </summary>
	/// <value>The position.</value>
	public Vector3 Position
	{
		get { return this.transform.position; }
		set
		{
			if (MapNav != null)
			{
				value.x = Mathf.Clamp(value.x, 0.5f, MapNav.gridWidth * MapNav.gridXNum - 0.5f);
				value.z = Mathf.Clamp(value.z, 1.0f, MapNav.gridHeight * MapNav.gridZNum - 4.0f);
			}
			this.transform.position = value;

			if (PositionChanged != null)
				PositionChanged(this);
		}
	}

	/// <summary>
	/// 角色逻辑格子位置
	/// </summary>
	public Pos Grid
	{
		get { return new Pos() { x = MapNav.GetGridX(Position), y = MapNav.GetGridZ(Position) }; }
		set { Position = MapNav.GetWorldPosition(value); }
	}


}
