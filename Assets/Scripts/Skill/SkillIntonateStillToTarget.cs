using UnityEngine;
using System.Collections;

/// <summary>
/// 吟唱，检测不可移动一段时间
/// </summary>
[RequireComponent(typeof(Skill))]
public class SkillIntonateStillToTarget : SkillBase {

	public float delay = 1.0f;

	private GameObject startGo;
	private Vector3 lastPosition;

	// Use this for initialization
	override public void StartSkill () {

		var skill = this.gameObject.GetComponent<Skill>();
		startGo = skill.startGo;
		lastPosition = startGo.transform.position;

		if(delay > 0.0f)
		{
			iTween.ValueTo(gameObject, iTween.Hash("from",delay,"to",0.0f,"time",delay,"onupdate", "onUpdate", "oncomplete","Finish", "oncompleteparams", true));
		}
		else
		{
			Finish(true);
		}		
	}

	void onUpdate(float delay) {}

	void Update()
	{
		if(!startGo)
		{
			Finish(false);
			return;
		}

		if(lastPosition != startGo.transform.position)
		{
			Finish(false);
			return;
		}

		lastPosition = startGo.transform.position;
	}

	/// <summary>
	/// 吟唱结束，如果成功就给服务器发送消息
	/// </summary>
	/// <param name="success">If set to <c>true</c> success.</param>
	void Finish(bool success)
	{
		if(success)
			Debug.Log("Intonate success，send msg to server");
		else
			Debug.Log("Intonate fail");
		Destroy(this);
	}
}
