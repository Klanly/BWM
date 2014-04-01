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

	public static SelectSceneEntryScriptUserCmd_CS Selected { get; set; }

	/// <summary>
	/// 场景点选
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static IEnumerator Execute(SelectSceneEntryScriptUserCmd_CS cmd)
	{
		Selected = cmd;
		var my = BattleScene.Instance.Gui<SelectTarget>();
		my.gameObject.SetActive(true);
		yield return null;
		switch (cmd.entry.entrytype)
		{
			case SceneEntryType.SceneEntryType_Npc:
				{
					Npc target;
					if (Npc.All.TryGetValue(cmd.entry.entryid, out target))
					{
						switch (target.TableInfo.Type)
						{
							case table.NpcType.Boss:
								my.Toggle<SelectTargetBoss>().Present(target);
								break;
							case table.NpcType.Elite:
								my.Toggle<SelectTargetElite>().Present(target);
								break;
							case table.NpcType.Monster:
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
					if (Role.All.TryGetValue(cmd.entry.entryid, out target))
						my.Toggle<SelectTargetRole>().Present(target);
				}
				break;
			default:
				break;
		}
	}

}
