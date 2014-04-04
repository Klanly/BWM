using UnityEngine;
using System.Collections;

/// <summary>
/// 播放动画
/// </summary>
[RequireComponent(typeof(Skill))]
public class SkillPlayStartAnimate : SkillBase
{

	public string action;

	override public void StartSkill()
	{
		if (string.IsNullOrEmpty(action))
		{
			Destroy(this);
			return;
		}

		var skill = gameObject.GetComponent<Skill>();
		if (skill && skill.startGo)
		{
			var animator = skill.startGo.GetComponent<Animator>();
			if (animator)
				animator.Play(action);
		}

		Destroy(this);
	}

}
