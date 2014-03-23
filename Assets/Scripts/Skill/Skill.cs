using UnityEngine;
using System.Collections;

/// <summary>
/// Skill.记录了公共信息，用于技能的其他部件使用
/// </summary>
public class Skill : SkillBase
{
	// TODO: 尽量使用TableSkill
	public int skillId;
	public GameObject startGo;
	public GameObject targetGo;

	#region 和技能相关的服务器下发的信息
	public int attackId;
	public int attackType;
	public int attackHp;

	public struct DefenceInfo
	{
		int defenceType;
		int defenceID;
		int definecHp;
	}
	public DefenceInfo[] defenceInfo;
	#endregion

	void Update()
	{
		// 所有部件都删除了，则删除自身
		if (GetComponents<SkillBase>().Length == 1)
		{
			Destroy(gameObject);
		}
	}
}
