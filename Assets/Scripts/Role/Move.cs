using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 可将“角色”行走至指定的世界坐标位置
/// </summary>
public class Move : MonoBehaviour
{
	/// <summary>
	/// 目标到达
	/// </summary>
	public delegate void TargetArrived();
	public TargetArrived targetArrived;

	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	private Entity entity;
	private Animator animator;

	private Vector3 targetPosition;
	/// <summary>
	/// 行走目标点
	/// </summary>
	/// <remarks>
	/// TODO: 不再以Vector3.zero作为无效标志
	/// </remarks>
	public Vector3 TargetPosition
	{
		get { return targetPosition; }
		set
		{
			if (MapNav != null)
			{
				value.x = Mathf.Clamp(value.x, 0, MapGrid.Width * MapNav.gridXNum);
				value.z = Mathf.Clamp(value.z, 0, MapGrid.Height * MapNav.gridZNum);
			}
			targetPosition = value;
			if (value != Vector3.zero)
			{
				if (animator && animator.GetFloat("speed") == 0.0f)
					animator.SetFloat("speed", entity.Speed);

				var relativePos = TargetPosition - entity.Position;
				this.transform.rotation = Quaternion.LookRotation(relativePos);
			}
			else
			{
				if (animator && animator.GetFloat("speed") > 0.0f)
					animator.SetFloat("speed", 0.0f);
			}
		}
	}

	/// <summary>
	/// 是否在移动中
	/// </summary>
	/// <returns><c>true</c>, if moving was ined, <c>false</c> otherwise.</returns>
	public bool InMoving()
	{
		return (TargetPosition != Vector3.zero);
	}

	/// <summary>
	/// 停止移动
	/// </summary>
	public void Stop()
	{
		TargetPosition = Vector3.zero;
	}

	// Use this for initialization
	void Start()
	{
		entity = gameObject.GetComponent<Entity>();
		animator = gameObject.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (InMoving())
		{
			Vector3 vDelta = TargetPosition - entity.Position;
			float fDeltaLen = vDelta.magnitude;
			vDelta.Normalize();

			Vector3 vOldPosition = entity.Position;
			float fMoveLen = entity.Speed * Time.deltaTime;
			bool bFinish = false;
			if (fMoveLen >= fDeltaLen)
			{
				fMoveLen = fDeltaLen;
				bFinish = true;
			}

			entity.Position = vOldPosition + vDelta * fMoveLen;
			if (bFinish)
			{
				Stop();
				//var oldRotate = this.transform.rotation;
				//this.transform.rotation = oldRotate;

				if (targetArrived != null)
					targetArrived();
			}
		}
	}

	/// <summary>
	/// 行走同步，发送行走请求消息到服务器
	/// </summary>
	/// <returns></returns>
	public bool Sync()
	{
		Net.Instance.Send(new Cmd.UserMoveUpMoveUserCmd_C()
		{
			poscm = new MapGrid(this.TargetPosition),
			angle = (uint)this.transform.localRotation.eulerAngles.y
		});
		return true;
	}
}
