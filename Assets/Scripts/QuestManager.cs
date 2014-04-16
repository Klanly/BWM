using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using Cmd;
using GX.Net;
using System.Collections.Generic;

public class QuestManager : IEnumerable<ClientQuest>
{
	public static QuestManager Instance { get; private set; }
	static QuestManager() { Instance = new QuestManager(); }

	private readonly List<ClientQuest> items = new List<ClientQuest>();

	public event Action<QuestManager> Changed;
	protected void OnChanged()
	{
		if (Changed != null)
			Changed(this);
		Debug.Log(this.ToString());
	}

	public ClientQuest this[uint questid] { get { return items.Find(i => i.squest.questid == questid); } }

	protected void Add(ClientQuest quest)
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
		return string.Join("\n", items.OrderBy(i => i.squest.questid).Select(i => i.ToString()).ToArray());
	}


	#region IEnumerable<ClientQuest> Members

	public IEnumerator<ClientQuest> GetEnumerator()
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
	public static void Execute(AddClientQuestListQuestUserCmd_S cmd)
	{
		QuestManager.Instance.items.Clear();
		QuestManager.Instance.items.AddRange(cmd.questlist);
		QuestManager.Instance.OnChanged();
	}

	[Execute]
	public static void Execute(AddClientQuestQuestUserCmd_S cmd)
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
	#endregion
}
