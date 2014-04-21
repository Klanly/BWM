using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using Cmd;
using GX.Net;
using System.Collections.Generic;

/// <remarks>客户端不应该对任务项的顺序做任何调整</remarks>
public class QuestManager : IEnumerable<QuestTrace>
{
	public static QuestManager Instance { get; private set; }
	static QuestManager() { Instance = new QuestManager(); }

	private readonly List<QuestTrace> items = new List<QuestTrace>();

	public event Action<QuestManager> Changed;
	protected void OnChanged()
	{
		if (Changed != null)
			Changed(this);
		Debug.Log(this.ToString());
	}

	public QuestTrace this[uint questid] { get { return items.Find(i => i.squest.questid == questid); } }

	protected void Add(QuestTrace quest)
	{
		var index = items.FindIndex(i => i.squest.questid == quest.squest.questid);
		if (index >= 0)
		{
			if (string.IsNullOrEmpty(quest.desc) == false)
				items[index].desc = quest.desc;
			items[index].squest = quest.squest;
		}
		else
		{
			items.Add(quest);
		}
	}

	protected void Add(SaveQuest quest)
	{
		var index = items.FindIndex(i => i.squest.questid == quest.questid);
		if (index >= 0)
		{
			items[index].squest = quest;
		}
		else
		{
			items.Add(new QuestTrace() { squest = quest, desc = string.Empty });
		}
	}

	protected bool Remove(uint questid)
	{
		var index = items.FindIndex(i => i.squest.questid == questid);
		if (index >= 0)
		{
			items.RemoveAt(index);
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		return string.Join("\n", items.Select(i => i.ToString()).ToArray());
	}


	#region IEnumerable<QuestTrace> Members

	public IEnumerator<QuestTrace> GetEnumerator()
	{
		return items.GetEnumerator();
	}

	#endregion

	#region IEnumerable Members

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	#endregion

	#region 网络消息处理
	[Execute]
	public static void Execute(AddQuestTraceQuestUserCmd_S cmd)
	{
		QuestManager.Instance.Add(cmd.quest);
		QuestManager.Instance.OnChanged();
	}

	[Execute]
	public static void Execute(RefreshQeustStateQuestUserCmd_S cmd)
	{
		var quest = QuestManager.Instance[cmd.squest.questid];
		if (quest != null)
		{
			quest.squest = cmd.squest;
			QuestManager.Instance.OnChanged();
		}
	}

	[Execute]
	public static void Execute(RemoveQuestQuestUserCmd_CS cmd)
	{
		if(QuestManager.Instance.Remove(cmd.questid))
			QuestManager.Instance.OnChanged();
	}

	[Execute]
	public static void Execute(ReturnQuestDetailInfoQuestUserCmd_S cmd)
	{
		QuestManager.Instance.Add(cmd.squest);
		QuestManager.Instance.OnChanged();

		var dlg = BattleScene.Instance.Gui<QuestDialog>();
		dlg.gameObject.SetActive(true);
		dlg.QuestDetail = cmd.detail;
		dlg.QuestID = cmd.squest.questid;
	}
	#endregion
}
