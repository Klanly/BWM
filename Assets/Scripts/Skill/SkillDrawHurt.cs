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
		var skill = gameObject.GetComponent<Skill>();
		if(skill)
		{
			foreach(var t in skill.hurts)
			{
				GameObject targetGo = null;
				switch (t.hurtid.entrytype)
				{
				case Cmd.SceneEntryType.SceneEntryType_Npc:
				{
					var target = Npc.All[t.hurtid.entryid];
					if (target != null)
					{
						target.SetHp(target.ServerInfo.hp + t.hp);
						targetGo = target.gameObject;
					}
				}
					break;
				case Cmd.SceneEntryType.SceneEntryType_Player:
				{
					if (t.hurtid.entryid == MainRole.Instance.Role.ServerInfo.charid)
					{
						MainRole.Instance.SetHp(MainRole.Instance.Role.ServerInfo.hp + t.hp);
						targetGo = MainRole.Instance.gameObject;
					}
					else
					{
						var target = Role.All[t.hurtid.entryid];
						target.SetHp(target.ServerInfo.hp + t.hp);
						targetGo = target.gameObject;
					}
				}
					break;
				}
				
				if (targetGo != null)
				{
					Debug.Log(skill.TableInfo.name + ":(" + skill.TableInfo.path + "):" + targetGo.name + ":hp:" + t.hp);
				}
			}
		}

		Destroy(this);
	}
}
