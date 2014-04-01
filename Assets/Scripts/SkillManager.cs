using UnityEngine;
using System.Collections;
using Cmd;
using System.Collections.Generic;
using GX.Net;
using System.Linq;
using System;

public class SkillManager : IEnumerable<KeyValuePair<uint, table.TableSkill>>
{
	/// <summary>
	/// 技能施法按钮数量
	/// </summary>
	public const int FireThumbsCount = 3;
	public static SkillManager Instance { get; private set; }
	static SkillManager() { Instance = new SkillManager(); }
	private readonly Dictionary<uint, table.TableSkill> skillLevels = new Dictionary<uint, table.TableSkill>();

	public event Action<SkillManager> SkillChanged;

	protected void OnSkillChanged()
	{
		if (SkillChanged != null)
			SkillChanged(this);
		Debug.Log(this.ToString());
	}

	void Clear()
	{
		this.skillLevels.Clear();
		// 对所有可能有的技能占位，方便访问
		foreach (var id in table.TableSkill.Where(MainRole.ServerInfo.userdata.profession))
			this.skillLevels[id] = null;
	}

	/// <summary>
	/// 得到给定<paramref name="skillID"/>对应的技能。如果未学习，返回null
	/// </summary>
	/// <param name="skillID"></param>
	/// <returns></returns>
	public table.TableSkill GetSkill(uint skillID)
	{
		table.TableSkill s;
		return skillLevels.TryGetValue(skillID, out s) ? s : null;
	}

	/// <summary>
	/// 得到给定技能学习到的等级，未学习的返回0
	/// </summary>
	/// <param name="skillID"></param>
	/// <returns></returns>
	public uint GetLevel(uint skillID)
	{
		var s = GetSkill(skillID);
		return s != null ? s.level : 0;
	}

	#region IEnumerable<KeyValuePair<uint,TableSkill>> Members

	public IEnumerator<KeyValuePair<uint, table.TableSkill>> GetEnumerator()
	{
		return this.skillLevels.GetEnumerator();
	}

	#endregion

	#region IEnumerable Members

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	#endregion

	public override string ToString()
	{
		return string.Join("\n", this.skillLevels.Select(i =>
		{
			var s = i.Value ?? table.TableSkill.First(i.Key);
			return string.Format("<color={0}>{1}:{2} {3}</color>",
				i.Value != null ? "green" : "orange",
				s.id, s.level, s.name);
		}).ToArray());
	}

	#region 网络消息处理
	[Execute]
	public static void Execute(AddSkillListSkillUserCmd_S cmd)
	{
		SkillManager.Instance.Clear();
		foreach (var s in cmd.skilllist)
			SkillManager.Instance.skillLevels[s.skillid] = s.TableInfo;
		SkillManager.Instance.OnSkillChanged();
	}
	[Execute]
	public static void Execute(AddSkillSkillUserCmd_S cmd)
	{
		SkillManager.Instance.skillLevels[cmd.skill.skillid] = cmd.skill.TableInfo;
		SkillManager.Instance.OnSkillChanged();
	}
	[Execute]
	public static void Execute(RemoveSkillSkillUserCmd_CS cmd)
	{
		SkillManager.Instance.skillLevels.Remove(cmd.skillid);
		SkillManager.Instance.OnSkillChanged();
	}
	#endregion
}
