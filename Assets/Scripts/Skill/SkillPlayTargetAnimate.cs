using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillPlayTargetAnimate : SkillBase
{

	public float delay;
	public string action;

	void ApplyTargetEvent()
	{
		if (delay > 0.0f)
		{
			iTween.ValueTo(gameObject, iTween.Hash(
				"from", delay, 
				"to", 0.0f, 
				"time", delay, 
				"onupdate", "onUpdate",
				"oncomplete", "PlayAnimate"));
		}
		else
		{
			PlayAnimate();
		}
	}

	void onUpdate(float delay) { }

	void PlayAnimate()
	{
		if (string.IsNullOrEmpty(action))
		{
			Destroy(this);
			return;
		}

		var skill = gameObject.GetComponent<Skill>();
		if (skill && skill.targetGo)
		{
			var animator = skill.targetGo.GetComponent<Animator>();
			if (animator)
				animator.Play(action);
		}
		Destroy(this);
	}
}
