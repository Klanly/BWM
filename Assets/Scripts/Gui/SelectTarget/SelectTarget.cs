using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Linq;

public class SelectTarget : MonoBehaviour
{
	/// <summary>
	/// 切换给定的SelectTargetX类型展示器可见，并返回其实例
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public T Toggle<T>() where T : MonoBehaviour
	{
		var present = default(T);
		foreach (var c in this.transform.GetAllChildren())
		{
			var p = c.gameObject.GetComponent<T>();
			c.gameObject.SetActive(p != null);
			if (p != null)
				present = p;
		}
		NGUITools.BringForward(present.gameObject);
		Debug.Log(string.Format("SelectTarget: <color=green>{0}</color>", typeof(T).Name));
		return present;
	}

	public static SceneEntryUid Selected { get; set; }

	/// <summary>
	/// 释放给定的技能
	/// </summary>
	/// <param name="skillID"></param>
	/// <returns></returns>
	public static bool FireSkill(uint skillID)
	{
		var skill = SkillManager.Instance.GetSkill(skillID);
		if (skill == null)
			return false;
		Debug.Log("FireSkill: " + skill);
		// TODO: 群攻搜索并批量发送攻击请求
		//var target = SelectTarget.Selected == null ? null : SelectTarget.Selected.entry.GetGameObject();
		var cmd = new RequestUseSkillUserCmd_C() { skillid = skill.id };
		if (SelectTarget.Selected != null && SelectTarget.Selected != null)
			cmd.hurts.Add(SelectTarget.Selected);
		Net.Instance.Send(cmd);
		//MainRole.Instance.castSkill.StartSkill("Prefabs/Skill/" + skill.path, target != null ? target.gameObject : null);
		return false;
	}

	/// <summary>
	/// 服务器驱动的技能生效
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static void Execute(ReturnUseSkillUserCmd_S cmd)
	{
		CastSkill cast = null;
		switch (cmd.owner.entrytype)
		{
			case Cmd.SceneEntryType.SceneEntryType_Npc:
				{
					Npc owner;
					if (Npc.All.TryGetValue(cmd.owner.entryid, out owner))
						cast = owner.CastSkill;
				}
				break;
			case Cmd.SceneEntryType.SceneEntryType_Player:
				{
					Role owner;
					if (Role.All.TryGetValue(cmd.owner.entryid, out owner))
						cast = owner.CastSkill;
				}
				break;
			default:
				break;
		}

		var skill = table.TableSkill.Where(cmd.skillid, cmd.skilllevel);
		if (cast == null || skill == null)
			return;
		foreach (var target in cmd.hurts)
			cast.StartSkill(skill, target);
	}

	/// <summary>
	/// 场景点选
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static IEnumerator Execute(SelectSceneEntryScriptUserCmd_CS cmd)
	{
		Selected = cmd.entry;
		var my = BattleScene.Instance.Gui<SelectTarget>();
		my.gameObject.SetActive(true);
		yield return null;
		switch (Selected.entrytype)
		{
			case SceneEntryType.SceneEntryType_Npc:
				{
					Npc target;
					if (Npc.All.TryGetValue(Selected.entryid, out target))
					{
						switch (target.TableInfo.BaseType)
						{
							case NpcBaseType.NpcBaseType_Boss:
								my.Toggle<SelectTargetBoss>().Present(target);
								break;
							case NpcBaseType.NpcBaseType_Elite:
								my.Toggle<SelectTargetElite>().Present(target);
								break;
							case NpcBaseType.NpcBaseType_Monster:
								my.Toggle<SelectTargetMonster>().Present(target);
								break;
							default:
								my.Toggle<SelectTargetNpc>().Present(target);
								break;
						}
					}
				}
				break;
			case SceneEntryType.SceneEntryType_Player:
				{
					Role target;
					if (Role.All.TryGetValue(Selected.entryid, out target))
						my.Toggle<SelectTargetRole>().Present(target);
				}
				break;
			default:
				break;
		}
	}

}
