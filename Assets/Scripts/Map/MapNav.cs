using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cmd;

[ExecuteInEditMode]
public class MapNav : MonoBehaviour
{
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
				return TileType.TileType_None;
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

	public Pos GetGrid(Vector3 worldPosition)
	{
		return new Pos(){x = GetGridX(worldPosition), y = GetGridZ(worldPosition)};
	}

	public Vector3 GetWorldPosition(Pos pos)
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

#if false
		// 禁用所有阻挡
		if (this.grids == null)
			Reset();
		for (var i = 0; i < grids.Length; i++)
			grids[i] = (TileType)0xFFFF;
#endif
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
				if (flag == TileType.TileType_None)
					Gizmos.color = new Color(0.5f, 0.0f, 0.0f, 0.5f);
				else
					Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

				Vector3 center = new Vector3(x * gridWidth + gridWidth * 0.5f, y, z * gridHeight + gridHeight * 0.5f);
				Vector3 size = new Vector3(gridWidth, 0, gridHeight);
				Gizmos.DrawCube(center, size);

				if ((flag & TileType.TileType_Walk) != 0)
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
	public const float ShortestMoveDst = 0.1f;
	/// <summary>
	/// 给服务器发送行走信息，最长路段长度
	/// </summary>
	private const float MaxPathLen = 5.0f;

	class PathNode
	{
		public int index;
		public int parentIndex;
		public float g;
		public float h;
		public float f;

		public Pos grid;
		public Vector3 position;

		public PathNode(int _index, int _parentIndex, float _g, float _h, MapNav mapNav)
		{
			index = _index;
			parentIndex = _parentIndex;
			g = _g;
			h = _h;
			f = g + h;

			grid = new Pos(){x = index % mapNav.gridXNum, y = index / mapNav.gridXNum};
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

	public List<Pos> GetPath(Vector3 fromPosition, Vector3 toPosition, TileType validType)
	{
		return GetPath(GetGridX(fromPosition), GetGridZ(fromPosition), GetGridX(toPosition), GetGridZ(toPosition), validType);
	}
	
	public List<Pos> GetPath(Pos fromPos, Pos toPos, TileType validType)
	{
		return GetPath(fromPos.x, fromPos.y, toPos.x, toPos.y, validType);
	}
		
	public List<Pos> GetPath(int fromGridX, int fromGridZ, int toGridX, int toGridZ, TileType validType)
	{
		List<Pos> path = new List<Pos>();
		if ((this[fromGridX, fromGridZ] & validType) == 0)
			return path;
		
		if ((this[toGridX, toGridZ] & validType) == 0)
			return path;

		// 同一个点也导出路径
		if(fromGridX == toGridX && fromGridZ == toGridZ)
		{
			path.Add(new Pos(){x = fromGridX, y = fromGridZ});
			path.Add(new Pos(){x = toGridX, y = toGridZ});
			return path;
		}

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

		List<Pos> tmpPath = new List<Pos>();
		PathNode node = closeDic[pnTo.index];
		while(node != null)
		{
			tmpPath.Add(node.grid);

			if(node.parentIndex == 0)
				break;
			node = totalDic[node.parentIndex];
		}

		path = LinePathEx(tmpPath, validType);
		return path;
	}

	/// <summary>
	/// 沿着指向源点的方向，把目标点前移一段距离
	/// </summary>
	/// <returns>The nearest grid.</returns>
	/// <param name="src">Source.</param>
	/// <param name="dst">Dst.</param>
	/// <param name="mindis">gridnum.</param>
	public Pos GetNearestGrid(Pos src, Pos dst, int gridnum=1)
	{
		Pos ptOut = src;
		if(src == dst)
			return ptOut;

		Vector3 vecDir = this.GetWorldPosition(src) - this.GetWorldPosition(dst);
		vecDir.Normalize();
		Vector3 vecOut = this.GetWorldPosition(dst) + gridnum * gridWidth * vecDir;
		ptOut = this.GetGrid(vecOut);
		return ptOut;
	}

	/// <summary>
	/// 在源点和目标点之间，找个有效的可到达点。搜寻范围在dst ~ (dst-radius)内
	/// </summary>
	/// <returns>The nearest valid grid.</returns>
	/// <param name="src">Source.</param>
	/// <param name="dst">Dst.</param>
	/// <param name="validType">Valid type.</param>
	/// <param name="radius">Grid Radius.</param>
	public Pos GetNearestValidGrid(Pos src, Pos dst, TileType validType, int gridRadius=-1)
	{
		Pos ptOut = dst;
		while((this[ptOut.x, ptOut.y] & validType) == 0)
		{
			ptOut = GetNearestGrid(src, ptOut);
			if(ptOut == src) break;
			if(gridRadius > 0)
			{
				gridRadius --;
				if(gridRadius == 0)
					break;
			}
		}

		if((this[ptOut.x, ptOut.y] & validType) != 0)
			return ptOut;
		else
			return null;
	}

	/// <summary>
	/// 把AStar算出的倒叙路径变正序，并拉直
	/// </summary>
	/// <returns>The path ex.</returns>
	/// <param name="srcPath">Source path.</param>
	private List<Pos> LinePathEx(List<Pos> srcPath, TileType validType = TileType.TileType_Walk)
	{
		List<Pos> dstPath = srcPath;
		dstPath.Reverse();

		int nStartIndex = 0;
		int nEndIndex = 0;
		int nSize = dstPath.Count;
		for(; nStartIndex < nSize - 2; ++nStartIndex)
		{
			for(nEndIndex = nStartIndex + 2; nEndIndex < nSize;)
			{
				if(LinePathClear(dstPath[nStartIndex], dstPath[nEndIndex], validType))
				{
					dstPath.RemoveRange(nStartIndex + 1, nEndIndex - nStartIndex - 1);
					nSize = dstPath.Count;
					nEndIndex = nStartIndex + 2;
				}
				else
				{
					nEndIndex ++;
				}
			}
		}

		nSize = dstPath.Count;
		nStartIndex = 0;
		nEndIndex = nStartIndex + 1;
		int nDelta = 0;
		for(; nEndIndex < nSize;)
		{
			nDelta = 0;

			List<Pos> path;
			SplitBlockLineEx(dstPath[nStartIndex], dstPath[nEndIndex], out path);
			if(path.Count >= 3)
			{
				path.RemoveAt(0);
				path.RemoveAt(path.Count-1);
				dstPath.InsertRange(nStartIndex + 1, path);
				nDelta = path.Count;
			}

			nSize += nDelta;
			nStartIndex = nEndIndex + nDelta;
			nEndIndex = nStartIndex + 1;
		}

		return dstPath;
	}

	/// <summary>
	/// 严格检查两点之间是否通畅
	/// </summary>
	/// <returns><c>true</c>, if path clear was lined, <c>false</c> otherwise.</returns>
	/// <param name="vecSrc">Vec source.</param>
	/// <param name="vecDst">Vec dst.</param>
	public bool LinePathClear(Vector3 vecSrc, Vector3 vecDst, TileType validType = TileType.TileType_Walk)
	{
		Pos dstPt = GetGrid(vecDst);

		Vector3 dir = vecDst - vecSrc;
		dir.Normalize();
		dir *= ShortestMoveDst;

		Vector3 curPos = vecSrc;
		Pos curPt = new Pos(){x=0, y=0};

		for(; (vecDst - curPos).magnitude > ShortestMoveDst; curPos += dir)
		{
			if(curPt != GetGrid(curPos))
			{
				curPt = GetGrid(curPos);
			}
			else
			{
				continue;
			}

			if((this[curPt.x, curPt.y] & validType) == 0)
			{
				return false;
			}

			if(curPt == dstPt)
				break;
		}

		return true;
	}

	public bool LinePathClear(Pos gridSrc, Pos gridDst, TileType validType = TileType.TileType_Walk)
	{
		return LinePathClear(GetWorldPosition(gridSrc), GetWorldPosition(gridDst), validType);
	}

	/// <summary>
	/// 两点间要是超过最大路径距离，就切分路段
	/// </summary>
	/// <param name="grid1">Grid1.</param>
	/// <param name="grid2">Grid2.</param>
	/// <param name="path">Path.</param>
	private void SplitBlockLineEx(Pos grid1, Pos grid2, out List<Pos> path)
	{
		path = new List<Pos>();
		if(grid1.x == grid2.x && grid1.y == grid2.y)
			return;

		path = new List<Pos>();
		path.Clear();

		Vector3 vecSrc = GetWorldPosition(grid1);
		Vector3 vecDst = GetWorldPosition(grid2);
		Vector3 dir = vecDst - vecSrc;
		float fLength = dir.magnitude;
		if(fLength < ShortestMoveDst)
			return;
		dir.Normalize();
		dir *= ShortestMoveDst;

		Vector3 curPos = vecSrc;
		Pos curPt = grid1;

		path.Add(grid1);
		for(;;)
		{
			curPos += dir;
			if(GetGrid(curPos) == curPt)
				continue;

			curPt = GetGrid(curPos);

			// 已经是最后一个节点，退出
			if(curPt == grid2)
			{
				if(path[path.Count-1] != grid2)
					path.Add(grid2);
				break;
			}

			// 大于一段路的最大长度，加入变换点
			if(path.Count > 0)
			{
				if((GetWorldPosition(curPt) - GetWorldPosition(path[path.Count-1])).magnitude >= MaxPathLen)
				{
					path.Add(curPt);
				}
			}
		}

		return;
	}

	/// <summary>
	/// 严格检查两点是否可达，若不可达，则修正目标点为最远可达点
	/// </summary>
	/// <returns><c>true</c> if this instance is path reached the specified vecSrc vecDst validType; otherwise, <c>false</c>.</returns>
	/// <param name="vecSrc">Vec source.</param>
	/// <param name="vecDst">Vec dst.</param>
	/// <param name="validType">Valid type.</param>
	public bool IsPathReached(Vector3 vecSrc, Vector3 vecOriginDst ,out Vector3 vecDst, TileType validType = TileType.TileType_Walk)
	{
		Pos ptSrc = GetGrid(vecSrc);
		Pos ptDst = GetGrid(vecOriginDst);

		Vector3 dir = vecOriginDst - vecSrc;
		float length = dir.magnitude;
		dir.Normalize();
		dir *= ShortestMoveDst;

		Vector3 vecCur = vecSrc;
		Pos ptCur = new Pos(){x=0, y=0};
		vecDst = vecCur;

		// 如果起始点是阻挡，检测方向是否为走出阻挡的方向
		if((this[ptSrc.x, ptSrc.y] & validType) == 0)
		{
			Vector3 vecForward = vecSrc + dir * gridWidth;
			Pos ptForward = GetGrid(vecSrc + dir * gridWidth);
			if(ptForward == ptSrc || (this[ptForward.x, ptForward.y] & validType) == 0)
				return false;
		}

		int i = 0;
		for(; (vecOriginDst - vecCur).magnitude > ShortestMoveDst; vecCur += dir)
		{
			if(ptCur != GetGrid(vecCur))
			{
				ptCur = GetGrid(vecCur);

				// 遇到阻挡，不考虑起始点的阻挡，有可能会走进去
				if((ptCur != ptSrc) && (this[ptCur.x, ptCur.y] & validType) == 0)
				{
					// 回退一小格，免得和阻挡格子太接近
					if(i <= 1) 
						vecDst = vecSrc;
					else
						vecDst -= dir;

					break;
				}
				
				// 到达目标点
				if(ptCur == ptDst)
				{
					vecDst = vecOriginDst;
					break;
				}
			}

			vecDst = vecCur;
			i++;
		}
		
		return (vecDst - vecSrc).magnitude > ShortestMoveDst;
	}

	#endregion

}
