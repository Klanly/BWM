using UnityEngine;
using System.Collections;

/// <summary>
/// 延迟给定时间后绘制伤害
/// </summary>
[RequireComponent(typeof(Skill))]
public class SkillDrawHurt : SkillBase
{
	public float delay;

	IEnumerator ApplyTargetEvent()
	{
		if (delay > 0.0f)
		{
			yield return new WaitForSeconds (delay);
			DrawHurt();
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
