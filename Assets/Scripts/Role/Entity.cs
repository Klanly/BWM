using UnityEngine;
using System.Collections;
using System;
using Cmd;


/// <summary>
/// 游戏场景对象基类
/// </summary>
public class Entity : MonoBehaviour
{

	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }

	/// <summary>
	/// 移动速度 m/s
	/// </summary>
	public float Speed = 5.0f;

	/// <summary>
	/// 世界坐标<see cref="Position"/>改变事件
	/// </summary>
	public event Action<Entity> PositionChanged;

	/// <summary>
	/// 世界坐标位置
	/// </summary>
	/// <value>The position.</value>
	public Vector3 Position
	{
		get { return this.transform.position; }
		set
		{
			if (MapNav != null)
			{
				// 确保角色对象在场景地图的可视范围内
				// TODO: 移至角色相关的部分
				value.x = Mathf.Clamp(value.x, 0.5f, MapGrid.Width * MapNav.gridXNum - 0.5f);
				value.z = Mathf.Clamp(value.z, 1.0f, MapGrid.Height * MapNav.gridZNum - 4.0f);
			}
			this.transform.position = value;

			if (PositionChanged != null)
				PositionChanged(this);
		}
	}

	/// <summary>
	/// 逻辑格子位置
	/// </summary>
	public MapGrid Grid
	{
		get { return new MapGrid() { x = MapNav.GetGridX(Position), z = MapNav.GetGridZ(Position) }; }
		set { Position = MapNav.GetWorldPosition(value); }
	}

	/// <summary>
	/// 当前可接受的阻挡类型
	/// </summary>
	/// <value>The type of the tile.</value>
	public TileType TileType	{ get { return TileType.TileType_Walk; } }


}
