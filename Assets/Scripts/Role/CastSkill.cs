using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 技能发射器，可指定目标释放技能
/// </summary>
public class CastSkill : MonoBehaviour
{
	public bool StartSkill(table.TableSkill skillTableInfo, List<Cmd.SkillHurtData> hurts)
	{
		var path = "Prefabs/Skill/" + skillTableInfo.path;
		var res = Resources.Load(path);
		if (res == null)
		{
			Debug.LogError("无法加载技能文件: " + path);
			return false;
		}

		var skill = Object.Instantiate(res) as GameObject;
		var s = skill.GetComponent<Skill>();
		s.startGo = gameObject;
		s.TableInfo = skillTableInfo;
		s.hurts = hurts;
		if (hurts.Count > 0)
		{
			var tg = hurts[0].hurtid.GetGameObject();
			if (tg != null)
			{
				s.targetGo = tg.gameObject;
			}
		}

		// 检查技能的有效性
		int count = 0;
		foreach(SendTargetEventBase t in skill.GetComponents<SendTargetEventBase>())
		{
			if (t.sendTargetEvent == true)
				count ++;
			
			if(count >= 2)
			{
				Debug.LogError("技能(" + path + ")有多个发送到达目标的组件");
				return false;
			}
		}

		foreach (SkillBase t in skill.GetComponents<SkillBase>())
		{
			t.StartSkill();
		}
		skill.transform.parent = transform;
		skill.transform.localPosition = Vector3.zero;
		return true;
	}
}
