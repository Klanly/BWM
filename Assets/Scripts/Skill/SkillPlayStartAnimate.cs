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
			// 面朝目标者
			if(skill.targetGo)
				skill.startGo.transform.LookAt(skill.targetGo.transform.position);

			var animator = skill.startGo.GetComponent<Animator>();
			if (animator)
			{
				//Debug.Log(action);
				animator.Play(action);
			}

			// 若在移动，则停止
			var move = skill.startGo.GetComponent<Move>();
			if (move != null && move.InMoving())
			{
				move.Stop();
			}
		}

		Destroy(this);
	}

}
