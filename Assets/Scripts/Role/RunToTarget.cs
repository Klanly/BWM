using UnityEngine;
using System.Collections;

/// <summary>
/// Run to target.
/// </summary>
public class RunToTarget : MonoBehaviour {

	public enum TargetType
	{
		Type_Null = 0,
		Type_GameObject,
		Type_Position,
	}
	public TargetType targetType = TargetType.Type_Null;

	/// <summary>
	/// 目标到达
	/// </summary>
	public delegate void TargetArrived();
	public TargetArrived targetArrived = null;

	private GameObject goTarget = null;
	private float radius;

	private Vector3 posTarget;

	private void clear()
	{
		targetType = TargetType.Type_Null;
		targetArrived = null;
		goTarget = null;

	}

	public void Target(GameObject _go, float _radius = 0.0f, TargetArrived _delegate = null)
	{
		clear();
		if(_go == null)
			return;

		targetType = TargetType.Type_GameObject;
		goTarget = _go;
		radius = _radius;
		targetArrived = _delegate;
	}

	public void Target(Vector3 pos, TargetArrived _delegate = null)
	{
		clear();
		targetType = TargetType.Type_Position;
		posTarget = pos;
		radius = 0.1f;
		targetArrived = _delegate;
	}

	void Update()
	{
		if (MainRole.Instance == null)
			return;

		bool bArrived = false;
		switch(targetType)
		{
		case TargetType.Type_Null:
			return;
		case TargetType.Type_GameObject:
		{
			if (goTarget == null)
			{
				MainRole.Instance.pathMove.StopPath();
				clear();
			}
			else
			{
				if (Vector3.Distance(goTarget.transform.position, MainRole.Instance.transform.position) <= radius)
				{
					bArrived = true;
				}
				else
				{
					MainRole.Instance.pathMove.WalkTo(goTarget.transform.position);
				}
			}
		}
			break;
		case TargetType.Type_Position:
		{
			if (Vector3.Distance(posTarget, MainRole.Instance.transform.position) <= radius)
			{
				bArrived = true;
			}
			else
			{
				MainRole.Instance.pathMove.WalkTo(posTarget);
			}
		}
			break;
		}

		if (bArrived)
		{
			if (targetArrived != null)
				targetArrived();
			clear();
		}
	}
}
