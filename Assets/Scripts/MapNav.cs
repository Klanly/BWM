using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MapNav : MonoBehaviour
{

	public float gridWidth = 1.0f;
	public float gridHeight = 1.0f;
	public int gridXNum = 0;
	public int gridZNum = 0;
	public bool showGrids = false;

	[SerializeField]
	public List<TileType> grids = new List<TileType>();

	/// <summary>
	/// 格子类型，0表示都不能走，每位0表示不可走，1表示可以走
	/// </summary>
	[System.Flags]
	public enum TileType
	{
		/// <summary>
		/// 可以行走
		/// </summary>
		Walk = 0x1,
		/// <summary>
		/// 可以游泳
		/// </summary>
		Water = 0x2,
	}
	/// <summary>
	/// 当前修改的阻挡标志
	/// </summary>
	public TileType curTileType = TileType.Walk;

	/// <summary>
	/// 格子颜色
	/// </summary>
	public static Color[] TileColor = 
	{
		Color.green,	// walk
		Color.cyan,		// water
		// etc
		// etc
	};

	/// <summary>
	/// 操作类型
	/// </summary>
	public enum ProcessType
	{
		/// <summary>
		/// 没有操作
		/// </summary>
		None = 0,
		/// <summary>
		/// 设置
		/// </summary>
		Set = 1,
		/// <summary>
		/// 清空
		/// </summary>
		Clear = 2,
	}
	/// <summary>
	/// 当前操作类型
	/// </summary>
	public ProcessType curProcessType = ProcessType.None;
	/// <summary>
	/// 操作半径
	/// </summary>
	public int radius = 1;


	/// <summary>
	/// Creates a new grid of tile nodes of x by y count
	/// </summary>
	public void Reset()
	{
		grids.Clear();
		grids.TrimExcess();
		int num = gridZNum * gridXNum;
		for (int i = 0; i < num; ++i)
			grids.Add(0);
	}

	/// <summary>
	/// get a grid at index of grids
	/// </summary>
	/// <param name="_x"></param>
	/// <param name="_z"></param>
	/// <returns></returns>
	public TileType this[int _x, int _z]
	{
		get
		{
			if (grids.Count == 0) return 0;
			if (_z < 0 && _z >= gridZNum) return 0;
			if (_x < 0 && _x >= gridXNum) return 0;
			return grids[_z * gridXNum + _x];
		}

		set
		{
			if (grids.Count == 0) return;
			if (_z < 0 && _z >= gridZNum) return;
			if (_x < 0 && _x >= gridXNum) return;
			grids[_z * gridXNum + _x] = value;
		}
	}

	/// <summary>
	/// shortcut to getting the length of grids.
	/// </summary>
	public int Length
	{
		get
		{
			return grids.Count;
		}
	}

	public int getX(Vector3 position)
	{
		return (int)(position.x / gridWidth);
	}

	public int getZ(Vector3 position)
	{
		return (int)(position.z / gridHeight);
	}

	public Vector3 getPosition(int x, int z)
	{
		if (x < 0) x = 0;
		if (x > gridXNum - 1) x = gridXNum - 1;
		if (z < 0) z = 0;
		if (z > gridZNum - 1) z = gridZNum - 1;
		return new Vector3((x + 0.5f) * gridWidth, 0.0f, (z + 0.5f) * gridHeight);
	}

	void OnDrawGizmos()
	{
		if (showGrids)
		{
			float y = 0.1f;
			for (int z = 0; z < gridZNum; ++z)
			{
				Gizmos.color = new Color(0, 0, 0, 0.5F);
				Gizmos.DrawLine(new Vector3(0, y, z * gridHeight), new Vector3(gridXNum * gridWidth, y, z * gridHeight));
			}

			for (int x = 0; x < gridXNum; ++x)
			{
				Gizmos.color = new Color(0, 0, 0, 0.5F);
				Gizmos.DrawLine(new Vector3(x * gridWidth, y, 0), new Vector3(x * gridWidth, y, gridZNum * gridHeight));
			}

			for (int z = 0; z < gridZNum; ++z)
			{
				for (int x = 0; x < gridXNum; ++x)
				{
					var flag = this[x, z];
					if (flag == 0)
						Gizmos.color = new Color(0.5f, 0.0f, 0.0f, 0.5f);
					else
						Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

					Vector3 center = new Vector3(x * gridWidth + gridWidth * 0.5f, y, z * gridHeight + gridHeight * 0.5f);
					Vector3 size = new Vector3(gridWidth, 0, gridHeight);
					Gizmos.DrawCube(center, size);

					if ((flag & TileType.Walk) > 0)
					{
						Gizmos.color = TileColor[0];
						center = new Vector3(x * gridWidth + gridWidth * 0.5f - gridWidth * 0.25f, y, z * gridHeight + gridHeight * 0.5f - gridHeight * 0.25f);
						size = new Vector3(gridWidth * 0.25f, 0, gridHeight * 0.25f);
						Gizmos.DrawCube(center, size);
					}
				}
			}
		}
	}
}
