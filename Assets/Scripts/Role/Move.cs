using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	private Entity entity;
	private Animator animator;

	private Vector3 targetPosition;
	/// <summary>
	/// 行走目标点
	/// </summary>
	/// <value>The target position.</value>
	public Vector3 TargetPosition
	{
		get { return targetPosition; }
		set
		{
			if (MapNav != null)
			{
				value.x = Mathf.Clamp(value.x, 0, MapNav.gridWidth * MapNav.gridXNum);
				value.z = Mathf.Clamp(value.z, 0, MapNav.gridHeight * MapNav.gridZNum);
			}
			targetPosition = value;
			if (value != Vector3.zero)
			{
				if (animator && animator.GetFloat("speed") == 0.0f)
					animator.SetFloat("speed", entity.Speed);
				
				var relativePos = TargetPosition - entity.Position;
				this.transform.rotation = Quaternion.LookRotation(relativePos);
			}
		}
	}


	// Use this for initialization
	void Start () 
	{
		entity = gameObject.GetComponent<Entity>();
		animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (TargetPosition != Vector3.zero)
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
				TargetPosition = Vector3.zero;
				//var oldRotate = this.transform.rotation;
				//this.transform.rotation = oldRotate;

				if(animator)
					animator.SetFloat("speed", 0.0f);
			}
		}	
	}
}
