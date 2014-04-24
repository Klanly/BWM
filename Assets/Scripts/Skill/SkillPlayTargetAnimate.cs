using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillPlayTargetAnimate : SkillBase
{
	public float delay;
	public string action;

	IEnumerator ApplyTargetEvent()
	{
		if (delay > 0.0f)
		{
			yield return new WaitForSeconds (delay);
			PlayAnimate();
		}
		else
		{
			PlayAnimate();
		}
	}

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
