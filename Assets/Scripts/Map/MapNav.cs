using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MapNav : MonoBehaviour
{
	/// <summary>
	/// 格子类型，0表示都不能走，每位0表示不可走，1表示可以走
	/// TODO: move to common, server and client use a same code.
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
	public float gridWidth = 0.25f;
	public int gridXNum = 128;
	public int gridZNum = 256;
	/// <summary>
	/// 索引方式：[z * gridXNum + x]
	/// </summary>
	public TileType[] grids;
	#endregion

	/// <summary>
	/// 显示格子
	/// </summary>
	public bool ShowGrids { get; set; }
	public float gridHeight { get { return gridWidth; } }

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
		Debug.LogWarning(string.Format("阻挡数据重置为 {0}*{1}", gridXNum, gridZNum));
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

	public Vector3 GetWorldPosition(Cmd.Pos pos)
	{
		return GetWorldPosition(pos.x, pos.y);
	}

	public Vector3 GetWorldPosition(int gridX, int gridZ)
	{
		return new Vector3(
			(Mathf.Clamp(gridX, 0, gridXNum - 1) + 0.5f) * gridWidth,
			0.0f,
			(Mathf.Clamp(gridZ, 0, gridZNum - 1) + 0.5f) * gridHeight);
	}

	void Start()
	{
		var width = gridXNum * gridWidth;
		var height = gridZNum * gridHeight;
		transform.localScale = new Vector3(width, height, 1);
		transform.localEulerAngles = new Vector3(90, 0, 0);
		transform.position = new Vector3(width * 0.5f, 0, height * 0.5f);
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


	#region AStar Find Path
	/// <summary>
	/// 给服务器发送行走信息，最长路段长度
	/// </summary>
	private const int PathSegment = 5;

	class PathNode
	{
		public int index;
		public int parentIndex;
		public float g;
		public float h;
		public float f;

		public Cmd.Pos grid;
		public Vector3 position;

		public PathNode(int _index, int _parentIndex, float _g, float _h, MapNav mapNav)
		{
			index = _index;
			parentIndex = _parentIndex;
			g = _g;
			h = _h;
			f = g + h;

			grid = new Cmd.Pos(){x = index % mapNav.gridXNum, y = index / mapNav.gridXNum};
			position = mapNav.GetWorldPosition(grid);
		}

		public void setG(float _g)
		{
			g = _g;
			f = g + h;
		}

		public override string ToString()
		{
			return string.Format("PathNode:" + index + ",(" + grid.x + "," + grid.y + "),(" + g + "," + h + "," + f + "),(" + position.x + "," + position.y + "," + position.z + ")");
		}
	}

	class NodeOffset
	{
		public int offset;
		public float distance;

		public NodeOffset(int _offset, float _distance)
		{
			offset = _offset;
			distance = _distance;
		}

		public override string ToString()
		{
			return string.Format("offset:" + offset + "," + distance);
		}
	}

	public List<Cmd.Pos> GetPath(Vector3 fromPosition, Vector3 toPosition, TileType validType)
	{
		return GetPath(GetGridX(fromPosition), GetGridZ(fromPosition), GetGridX(toPosition), GetGridZ(toPosition), validType);
	}
	
	public List<Cmd.Pos> GetPath(Cmd.Pos fromPos, Cmd.Pos toPos, TileType validType)
	{
		return GetPath(fromPos.x, fromPos.y, toPos.x, toPos.y, validType);
	}
		
	public List<Cmd.Pos> GetPath(int fromGridX, int fromGridZ, int toGridX, int toGridZ, TileType validType)
	{
		List<Cmd.Pos> path = new List<Cmd.Pos>();
		if ((this[fromGridX, fromGridZ] & validType) == 0)
			return path;
		
		if ((this[toGridX, toGridZ] & validType) == 0)
			return path;

		PathNode pnFrom = new PathNode(fromGridZ * gridXNum + fromGridX, 0, 0, 0, this);
		PathNode pnTo = new PathNode(toGridZ * gridXNum + toGridX, 0, 0, 0, this);

		Dictionary<int, PathNode> totalDic = new Dictionary<int, PathNode>();
		Dictionary<int, PathNode> openDic = new Dictionary<int, PathNode>();
		Dictionary<int, PathNode> closeDic = new Dictionary<int, PathNode>();
		openDic.Add(pnFrom.index, pnFrom);
		totalDic.Add(pnFrom.index, pnFrom);

		float disCornor = Mathf.Sqrt(gridWidth * gridWidth + gridHeight * gridHeight);
		NodeOffset[] offsets = new NodeOffset[]{new NodeOffset(gridXNum-1, disCornor), new NodeOffset(gridXNum, gridHeight),new NodeOffset(gridXNum+1, disCornor),
			new NodeOffset(-1, gridWidth), new NodeOffset(1, gridWidth),
			new NodeOffset(-gridXNum-1, disCornor), new NodeOffset(-gridXNum, gridHeight),new NodeOffset(-gridXNum+1, disCornor)};
		while(openDic.Count > 0)
		{
			PathNode pn = null;
			foreach(KeyValuePair<int, PathNode> t in openDic)
			{
				if (pn == null) pn = t.Value;
				if (t.Value.f < pn.f) pn = t.Value;
			}

			closeDic.Add(pn.index, pn);
			openDic.Remove(pn.index);

			if (pn.index == pnTo.index) break;

			foreach(NodeOffset offset in offsets)
			{
				int index = pn.index + offset.offset;
				if (index < 0 || index > grids.Length) continue;
				if (closeDic.ContainsKey(index)) continue;
				if ((grids[index] & validType) == 0) continue;

				PathNode pntmp = null;
				float g = pn.g + offset.distance;

				if (openDic.ContainsKey(index)) 
				{
					pntmp = openDic[index];
					if (g < pntmp.g)
					{
						pntmp.parentIndex = pn.index;
						pntmp.setG(g);
					}
				}
				else
				{
					pntmp = new PathNode(index, pn.index, 0, 0, this);
					openDic[index] = pntmp;
					totalDic[index] = pntmp;
					pntmp.h = Mathf.Abs(Vector3.Distance(pntmp.position, pnTo.position));
					pntmp.setG(g);
				}
			}
		}

		if(!closeDic.ContainsKey(pnTo.index))
			return path;

		List<Cmd.Pos> tmpPath = new List<Cmd.Pos>();
		PathNode node = closeDic[pnTo.index];
		while(node != null)
		{
			tmpPath.Add(node.grid);

			if(node.parentIndex == 0)
				break;
			node = totalDic[node.parentIndex];
		}
		tmpPath.Reverse();

		// 去除中间相同方向的点，同时一段路径不能超过PathSegment长
		int olddx = 0;
		int olddy = 0;
		int dx = 0;
		int dy = 0;
		int segment = 0;
		foreach(Cmd.Pos grid in tmpPath)
		{
			if(path.Count < 2)
			{
				path.Add(grid);
			}
			else
			{
				if (olddx == 0 && olddy == 0)
				{
					olddx = path[path.Count-1].x - path[path.Count-2].x;
					olddy = path[path.Count-1].y - path[path.Count-2].y;
					segment = 1;
				}

				dx = grid.x - path[path.Count-1].x;
				dy = grid.y - path[path.Count-1].y;
				if (olddx == dx && olddy == dy && segment < PathSegment)
				{
					path[path.Count-1] = grid;
					segment ++;
				}
				else
				{
					olddx = dx;
					olddy = dy;
					path.Add(grid);
					segment = 1;
				}
			}
		}

		return path;
	}
	#endregion
}
