using UnityEngine;
using System.Collections;
using Cmd;
using System.Collections.Generic;
using GX.Net;
using System.Linq;

public class SkillManager : IEnumerable<SaveSkill>
{
	public static SkillManager Instance { get; private set; }
	static SkillManager() { Instance = new SkillManager(); }
	private readonly List<SaveSkill> skills = new List<SaveSkill>();

	#region IEnumerable<SaveSkill> Members

	public IEnumerator<SaveSkill> GetEnumerator()
	{
		return skills.GetEnumerator();
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
		return string.Join("\n", this.OrderBy(i => i.skillid).Select(i => i.ToString()).ToArray());
	}

	#region 网络消息处理
	[Execute]
	public static void Execute(AddSkillListSkillUserCmd_S cmd)
	{
		SkillManager.Instance.skills.Clear();
		SkillManager.Instance.skills.AddRange(cmd.skilllist);
		Debug.Log(SkillManager.Instance);
	}
	[Execute]
	public static void Execute(AddSkillSkillUserCmd_S cmd)
	{
		SkillManager.Instance.skills.RemoveAll(i => i.skillid == cmd.skill.skillid);
		SkillManager.Instance.skills.Add(cmd.skill);
		Debug.Log(SkillManager.Instance);
	}
	[Execute]
	public static void Execute(RemoveSkillSkillUserCmd_CS cmd)
	{
		SkillManager.Instance.skills.RemoveAll(i => i.skillid == cmd.skillid);
		Debug.Log(SkillManager.Instance);
	}
	#endregion
}
