using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cmd;
using System;

/// <summary>
/// 自动寻路移动
/// </summary>
[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Move))]
public class PathMove : MonoBehaviour {

	public delegate void PathFinished();
	public PathFinished pathFinished;

	private List<MapGrid> path = new List<MapGrid>();
	//private Vector3 src;
	private Vector3 dst;

	private Entity entity;
	private Move move;
	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }

	// Use this for initialization
	void Start () {
		entity = this.gameObject.GetComponent<Entity>();
		move = this.gameObject.GetComponent<Move>();
	}

	public void WalkTo(Vector3 _dst, PathFinished _delegate = null)
	{
		MapGrid gridOriginSrc = entity.Grid;
		MapGrid gridOriginDst = new MapGrid(_dst);

		// 如果起始点是阻挡，检测方向是否为走出阻挡的方向
		MapGrid gridRealSrc = gridOriginSrc;
		if((MapNav[gridRealSrc.x, gridRealSrc.z] & entity.TileType) == 0)
		{
			gridRealSrc = MapNav.GetNearestValidGrid(gridOriginDst, gridOriginSrc, entity.TileType, 1);
			if(gridRealSrc == null)
			{
				StopPath();
				return;
			}
		}

		MapGrid gridRealDst = MapNav.GetNearestValidGrid(gridRealSrc, gridOriginDst, entity.TileType);
		if (gridRealDst == null)
		{
			StopPath();
			return;
		}

		// 正在走着
		if (path.Count > 0 && path[path.Count-1] == gridRealDst)
		{
			pathFinished = null;
			pathFinished += _delegate;
			return;
		}

		path = MapNav.GetPath(gridRealSrc, gridRealDst, entity.TileType);
		if(path.Count == 0)
		{
			StopPath();
			return;
		}

		//src = entity.Position;
		if (gridRealDst == gridOriginDst)
			dst = _dst;
		else
			dst = MapNav.GetWorldPosition(gridRealDst);
		move.targetArrived -= this.onTargetArrived;
		move.targetArrived += this.onTargetArrived;
		pathFinished = null;
		pathFinished += _delegate;

		// 路径第一个节点为起始点，放弃，从第二个节点开始
		path.RemoveAt(0);
		onTargetArrived();
	}

	void onTargetArrived()
	{
		// 路径走完
		if (path.Count == 0)
		{
			PathFinished tmpDelegate = pathFinished;
			StopPath();

			if(tmpDelegate != null)
				tmpDelegate();
		}
		// 最后一个节点
		else if (path.Count == 1)
		{
			move.TargetPosition = dst;
			path.RemoveAt(0);
			//Debug.Log("发送移动消息:dst:" + move.TargetPosition + ",dir:" + this.transform.rotation);
		}
		// 中间节点
		else
		{
			move.TargetPosition = MapNav.GetWorldPosition(path[0]);
			path.RemoveAt(0);
			//Debug.Log("发送移动消息:dst:" + move.TargetPosition + ",dir:" + this.transform.rotation);
		}
	}

	public void StopPath()
	{
		if(path.Count > 0)
		{
			path.Clear();
			move.targetArrived -= this.onTargetArrived;
			pathFinished = null;
		}
	}
}
