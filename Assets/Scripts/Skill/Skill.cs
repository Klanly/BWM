using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Skill.记录了公共信息，用于技能的其他部件使用
/// </summary>
public class Skill : SkillBase
{
	/// <summary>
	/// 攻击者
	/// </summary>
	public GameObject startGo;

	/// <summary>
	/// 第一被击者，用于攻击特效飞向目标
	/// </summary>
	public GameObject targetGo;

	/// <summary>
	/// 所有被击者，包括第一被击者
	/// </summary>
	public List<Cmd.SkillHurtData> hurts = new List<Cmd.SkillHurtData>();
	public List<GameObject> targetGos = new List<GameObject>();

	/// <summary>
	/// 技能信息
	/// </summary>
	/// <value>The table info.</value>
	public table.TableSkill TableInfo { get; set; }

	void Update()
	{
		// 所有部件都删除了，则删除自身
		if (GetComponents<SkillBase>().Length == 1)
		{
			Destroy(gameObject);
		}
	}
}
