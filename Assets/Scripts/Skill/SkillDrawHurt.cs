using UnityEngine;
using System.Collections;

/// <summary>
/// 延迟给定时间后绘制伤害
/// </summary>
[RequireComponent(typeof(Skill))]
public class SkillDrawHurt : SkillBase
{

	public float delay;

	void ApplyTargetEvent()
	{
		if (delay > 0.0f)
		{
			iTween.ValueTo(gameObject, iTween.Hash(
				"from", delay, 
				"to", 0.0f, 
				"time", delay, 
				"oncomplete", "DrawHurt"));
		}
		else
		{
			DrawHurt();
		}
	}

	/// <summary>
	/// 绘制伤害
	/// </summary>
	void DrawHurt()
	{
		Debug.Log("hp:" + 10);
		Destroy(this);
	}
}
