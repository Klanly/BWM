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
						OnSelect(target);
				}
				break;
			case SceneEntryType.SceneEntryType_Player:
				{
					Role target;
					if (Role.All.TryGetValue(Selected.entryid, out target))
						my.Toggle<SelectTargetRole>().OnSelect(target);
				}
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// 选择指定的NPC头像
	/// </summary>
	/// <param name="target"></param>
	private static void OnSelect(Npc target)
	{
		var my = BattleScene.Instance.Gui<SelectTarget>();
		switch (target.TableInfo.BaseType)
		{
			case NpcBaseType.NpcBaseType_Boss:
				my.Toggle<SelectTargetBoss>().OnSelect(target);
				break;
			case NpcBaseType.NpcBaseType_Elite:
				my.Toggle<SelectTargetElite>().OnSelect(target);
				break;
			case NpcBaseType.NpcBaseType_Monster:
				my.Toggle<SelectTargetMonster>().OnSelect(target);
				break;
			default:
				my.Toggle<SelectTargetNpc>().OnSelect(target);
				break;
		}
		OnUpdate(target);
	}

	/// <summary>
	/// 更新指定的NPC头像信息
	/// </summary>
	/// <param name="target"></param>
	/// <returns></returns>
	public static bool OnUpdate(Npc target)
	{
		if (SelectTarget.Selected.entrytype != SceneEntryType.SceneEntryType_Npc || SelectTarget.Selected.entryid != target.ServerInfo.tempid)
			return false;
		switch (target.TableInfo.BaseType)
		{
			case NpcBaseType.NpcBaseType_Boss:
				BattleScene.Instance.Gui<SelectTargetBoss>().OnUpdate(target);
				return true;
			case NpcBaseType.NpcBaseType_Elite:
				BattleScene.Instance.Gui<SelectTargetElite>().OnUpdate(target);
				return true;
			case NpcBaseType.NpcBaseType_Monster:
				BattleScene.Instance.Gui<SelectTargetMonster>().OnUpdate(target);
				return true;
			default:
				return false;
		}
	}

}
