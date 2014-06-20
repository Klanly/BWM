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
						if(target.ServerInfo.hp == t.prehp)
							target.SetHp(target.ServerInfo.hp - t.subhp);
						targetGo = target.gameObject;
					}
				}
					break;
				case Cmd.SceneEntryType.SceneEntryType_Player:
				{
					if (t.hurtid.entryid == MainRole.Instance.Role.ServerInfo.charid)
					{
						if (MainRole.Instance.Role.ServerInfo.hp == t.prehp)
						{
							MainRole.Instance.SetHp(MainRole.Instance.Role.ServerInfo.hp - t.subhp);
						}
						targetGo = MainRole.Instance.gameObject;
					}
					else
					{
						var target = Role.All[t.hurtid.entryid];
						if (target.ServerInfo.hp == t.prehp)
							target.SetHp(target.ServerInfo.hp - t.subhp);
						targetGo = target.gameObject;
					}
				}
					break;
				}
				
				if (targetGo != null)
				{
					Debug.Log(skill.TableInfo.name + ":(" + skill.TableInfo.path + "):" + targetGo.name + ":hp:" + t.subhp);

					var gohp = Instantiate(Resources.Load("Prefabs/Gui/HurtTipHp")) as GameObject;
					var labelhp = gohp.GetComponentInChildren<UILabel>();
					if (t.subhp > 0)
						labelhp.text = "-" + t.subhp.ToString();
					else if (t.subhp < 0)
						labelhp.text = "+" + (-1 * t.subhp).ToString();
					gohp.GetComponent<UIWidget>().SetAnchor(targetGo);
				}
			}
		}

		Destroy(this);
	}
}
