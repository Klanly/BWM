using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MapNav : MonoBehaviour {

	public float gridWidth = 1.0f;
	public float gridHeight = 1.0f;
	public int gridXNum = 0;
	public int gridZNum = 0;
	public bool showGrids = false;
	public uint[] grids;

	// 格子类型，0表示都不能走，每位0表示不可走，1表示可以走
	public enum TileType
	{
		Walk	= 0x1,	// 可以行走
		Water	= 0x2,	// 可以游泳
	}
	public TileType curTileType = TileType.Walk;			// 当前修改的阻挡标志

	// 格子颜色
	public static Color[] TileColor = 
	{
		Color.green,	// walk
		Color.cyan,		// water
		// etc
		// etc
	};

	// 操作类型
	public enum ProcessType
	{
		None 	= 0,	// 没有操作
		Set		= 1,	// 设置
		Clear	= 2,	// 清空
	}
	public ProcessType curProcessType = ProcessType.None;	// 当前操作类型
	public int radius = 1;									// 操作半径


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Creates a new grid of tile nodes of x by y count
	/// </summary>
	public void Reset()
	{
		grids = new uint[gridZNum * gridXNum];
		for(int i = 0; i < grids.Length; ++i)
			grids[i] = 0;
	}

	// get a grid at index of grids
	public uint this[int _x, int _z]
	{ 
		get 
		{
			if(grids == null) return 0;
			if(_z < 0 && _z >= gridZNum) return 0;
			if(_x < 0 && _x >= gridXNum) return 0;
			return grids[_z * gridXNum + _x];
		} 

		set
		{
			if(grids == null) return;
			if(_z < 0 && _z >= gridZNum) return;
			if(_x < 0 && _x >= gridXNum) return;
			grids[_z * gridXNum + _x] = value;
		}
	}
	
	// shortcut to getting the length of grids.
	public int Length 
	{ 
		get 
		{
			if (grids == null) return 0;
			return grids.Length; 
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
		if(x < 0) x = 0;
		if(x > gridXNum-1) x = gridXNum - 1;
		if(z < 0) z = 0;
		if(z > gridZNum-1) z = gridZNum - 1;
		return new Vector3((x + 0.5f) * gridWidth, 0.0f, (z + 0.5f) * gridHeight);
	}

	void OnDrawGizmos() 
	{
		if(showGrids)
		{
			float y = 0.1f;
			for(int z = 0; z < gridZNum; ++z)
			{
				Gizmos.color = new Color(0, 0, 0, 0.5F);
				Gizmos.DrawLine(new Vector3(0, y, z * gridHeight), new Vector3(gridXNum * gridWidth, y, z * gridHeight));
			}

			for(int x = 0; x < gridXNum; ++x)
			{
				Gizmos.color = new Color(0, 0, 0, 0.5F);
				Gizmos.DrawLine(new Vector3(x * gridWidth, y, 0), new Vector3(x * gridWidth, y, gridZNum * gridHeight));
			}

			for(int z = 0; z < gridZNum; ++z)
			{
				for(int x = 0; x < gridXNum; ++x)
				{
					uint flag = this[x,z];
					if(flag == 0)
						Gizmos.color = new Color(0.5f, 0.0f, 0.0f, 0.5f);
					else
						Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

					Vector3 center = new Vector3(x * gridWidth + gridWidth * 0.5f, y, z * gridHeight + gridHeight * 0.5f);
					Vector3 size = new Vector3(gridWidth, 0, gridHeight);
					Gizmos.DrawCube(center, size);

					if((flag & (uint)TileType.Walk) > 0)
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
