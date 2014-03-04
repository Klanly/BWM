using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MapNav : MonoBehaviour
{
	/// <summary>
	/// 格子类型，0表示都不能走，每位0表示不可走，1表示可以走
	/// </summary>
	[System.Flags]
	public enum TileType
	{
		None = 0,
		/// <summary>
		/// 可以行走
		/// </summary>
		Walk = 0x1,
		/// <summary>
		/// 可以游泳
		/// </summary>
		Water = 0x2,
	}

	#region SerializedProperty
	public float gridWidth = 1.0f;
	public float gridHeight = 1.0f;
	public int gridXNum = 0;
	public int gridZNum = 0;
	/// <summary>
	/// 索引方式：[z * gridXNum + x]
	/// </summary>
	public TileType[] grids;
	#endregion

	/// <summary>
	/// 显示格子
	/// </summary>
	public bool ShowGrids { get; set; }

	/// <summary>
	/// get a grid at index of grids
	/// </summary>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public TileType this[int x, int z]
	{
		get
		{
			var index = z * gridXNum + x;
			if (grids == null || index < 0 || index > grids.Length)
				return TileType.None;
			return grids[index];
		}
		set
		{
			var index = z * gridXNum + x;
			if (grids == null || index < 0 || index > grids.Length)
				return;
			grids[index] = value;
		}
	}

	/// <summary>
	/// Creates a new grid of tile nodes of x by y count
	/// </summary>
	public void Reset()
	{
		grids = new TileType[gridZNum * gridXNum];
	}

	public int GetGridX(Vector3 worldPosition)
	{
		return (int)(worldPosition.x / gridWidth);
	}

	public int GetGridZ(Vector3 worldPosition)
	{
		return (int)(worldPosition.z / gridHeight);
	}

	public Vector3 GetWorldPosition(int gridX, int gridZ)
	{
		return new Vector3(
			(Mathf.Clamp(gridX, 0, gridXNum - 1) + 0.5f) * gridWidth,
			0.0f,
			(Mathf.Clamp(gridZ, 0, gridZNum - 1) + 0.5f) * gridHeight);
	}

	void OnDrawGizmos()
	{
		if (ShowGrids == false)
			return;
		// 格子颜色
		var tileColor = new Color[]
		{
			Color.green,	// walk
			Color.cyan,		// water
			// etc
			// etc
		};

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
				if (flag == TileType.None)
					Gizmos.color = new Color(0.5f, 0.0f, 0.0f, 0.5f);
				else
					Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

				Vector3 center = new Vector3(x * gridWidth + gridWidth * 0.5f, y, z * gridHeight + gridHeight * 0.5f);
				Vector3 size = new Vector3(gridWidth, 0, gridHeight);
				Gizmos.DrawCube(center, size);

				if ((flag & TileType.Walk) != 0)
				{
					Gizmos.color = tileColor[0];
					center = new Vector3(x * gridWidth + gridWidth * 0.5f - gridWidth * 0.25f, y, z * gridHeight + gridHeight * 0.5f - gridHeight * 0.25f);
					size = new Vector3(gridWidth * 0.25f, 0, gridHeight * 0.25f);
					Gizmos.DrawCube(center, size);
				}
			}
		}
	}
}
