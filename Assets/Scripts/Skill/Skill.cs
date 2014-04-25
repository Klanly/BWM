using UnityEngine;
using System.Collections;

/// <summary>
/// Skill.记录了公共信息，用于技能的其他部件使用
/// </summary>
public class Skill : SkillBase
{
	public GameObject startGo;
	public GameObject targetGo;

	public table.TableSkill TableInfo { get; set; }
	public Cmd.SkillHurtData Hurt { get; set; }

	void Update()
	{
		// 所有部件都删除了，则删除自身
		if (GetComponents<SkillBase>().Length == 1)
		{
			Destroy(gameObject);
		}
	}
}
