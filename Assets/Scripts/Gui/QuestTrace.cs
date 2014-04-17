using UnityEngine;
using System.Collections;
using System.Linq;
using Cmd;

/// <summary>
/// 任务追踪界面
/// </summary>
public class QuestTrace : MonoBehaviour
{
	public UIXmlRichText uiItemProto;
	public GameObject uiContent;
	public GameObject uiBackground;

	// Use this for initialization
	IEnumerator Start()
	{
		uiItemProto.gameObject.SetActive(false);
		yield return new WaitForEndOfFrame();
		QuestManager.Instance.Changed += OnQuestChanged;
		OnQuestChanged(QuestManager.Instance);
		
	}

	void OnDestroy()
	{
		QuestManager.Instance.Changed -= OnQuestChanged;
	}

	private void OnQuestChanged(QuestManager quests)
	{
		StartCoroutine(Present(quests));
	}

	IEnumerator Present(QuestManager quests)
	{
		uiContent.SetActive(quests.Any());
		uiBackground.SetActive(uiContent.activeSelf);
		uiContent.transform.DestroyAllChildren();

		if (quests.Any())
		{
			var list = quests.Select(q => Tuple.Create(NGUITools.AddChild(uiContent, uiItemProto.gameObject), q)).ToList();
			foreach (var q in list)
			{
				q.Item1.name = q.Item2.squest.questid.ToString();
				UIEventListener.Get(q.Item1).onClick = go => OnQuestTraceClicked(q.Item2);
				q.Item1.SetActive(true);
			}
			yield return new WaitForEndOfFrame();

			var height = 0;
			foreach (var _ in list)
			{
				var q = _;
				q.Item1.transform.localPosition = new Vector3(0, -height, 0);
				q.Item1.GetComponent<UIXmlRichText>().AddXml(q.Item2.desc);
				height += q.Item1.GetComponent<UIWidget>().height;
			}
			uiContent.GetComponent<UIWidget>().height = height;
		}
	}

	private void OnQuestTraceClicked(ClientQuest quest)
	{
		//if (quest.npcbaseid == 0)
		//{
		//	Net.Instance.Send(new RequestQuestDetailInfoQuestUserCmd_C()
		//	{
		//		questid = quest.npcbaseid,
		//	});
		//}
		//else
		//{
		//	var npc = Npc.All.Values.FirstOrDefault(i => i.TableInfo.id == quest.npcbaseid);
		//	if (npc == null)
		//	{
		//		Debug.Log("任务中无法找到指定的npc: " + quest);
		//	}
		//	else
		//	{
		//		MainRole.Instance.pathMove.WalkTo(npc.transform.localPosition, () =>
		//			Net.Instance.Send(new RequestQuestDetailInfoQuestUserCmd_C()
		//			{
		//				questid = quest.npcbaseid,
		//			}));
		//	}
		//}
	}
}
