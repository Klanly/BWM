using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillDelayToTarget : SendTargetEventBase
{
	public float delay = 1.0f;

	// Use this for initialization
	override public void StartSkill()
	{
	}

	void Update()
	{
		delay -= Time.deltaTime;
		if (delay <= 0.0f)
		{
			StartTargetEvent();
		}
	}


	void StartTargetEvent()
	{
		if (sendTargetEvent)
		{
			gameObject.SendMessage("ApplyTargetEvent");
		}
		Destroy(this);
	}
}
