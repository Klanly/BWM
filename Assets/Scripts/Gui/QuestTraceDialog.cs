﻿using UnityEngine;
using System.Collections;
using System.Linq;
using Cmd;
using System.Xml.Linq;
using GX.Net;
using GX;

/// <summary>
/// 任务追踪界面
/// </summary>
public class QuestTraceDialog : MonoBehaviour
{
	public UIXmlRichText uiItemProto;
	public GameObject uiContent;
	public GameObject uiBackground;

	// Use this for initialization
	IEnumerator Start()
	{
		uiItemProto.gameObject.SetActive(false);
		uiItemProto.protoLabel.GetComponent<UIWidget>().color = "#D3BDA6FF".Parse(Color.white);
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
				q.Item1.GetComponent<UIXmlRichText>().AddXml(q.Item2.TraceContent);
				height += q.Item1.GetComponent<UIWidget>().height;
			}
			uiContent.GetComponent<UIWidget>().height = height;
		}
	}

	private void OnQuestTraceClicked(QuestTrace quest)
	{
		WebSocketClient.Instance.Send(new RequestClickQuestTraceQuestUserCmd_C() { questid = quest.squest.questid });
	}

	[Execute]
	public static void Execute(ReturnClickQuestTraceQuestUserCmd_S cmd)
	{
		if (BattleScene.Instance == null)
			return;
		switch (cmd.@event)
		{
			case ClickQuestTaceEvent.ClickQuestTaceEvent_None:
				break;
			case ClickQuestTaceEvent.ClickQuestTaceEvent_GoToNpc:
				{
					var npc = (from i in Npc.All where i.Value.TableInfo.id == cmd.npcbaseid select i.Value).FirstOrDefault();
					if (npc == null)
						break;
					var position = npc.transform.localPosition;
					// TODO: 需要随机到10格范围内，避免寻路重叠
					if (cmd.repeatclick)
					{
						MainRole.Instance.pathMove.WalkTo(position, () =>
							WebSocketClient.Instance.Send(new RequestClickQuestTraceQuestUserCmd_C() { questid = cmd.questid }));
					}
					else
					{
						MainRole.Instance.pathMove.WalkTo(position);
					}
				}
				break;
			case ClickQuestTaceEvent.ClickQuestTaceEvent_GoToPositon:
				{
					// TODO: process for cmd.position.countryid and cmd.position.mapid
					var position = BattleScene.Instance.MapNav.GetWorldPosition(new MapGrid(cmd.position.pos));
					if (cmd.repeatclick)
					{
						MainRole.Instance.pathMove.WalkTo(position, () =>
							WebSocketClient.Instance.Send(new RequestClickQuestTraceQuestUserCmd_C() { questid = cmd.questid }));
					}
					else
					{
						MainRole.Instance.pathMove.WalkTo(position);
					}
				}
				break;
			case ClickQuestTaceEvent.ClickQuestTaceEvent_OpenDialog:
				{
					var gui = BattleScene.Instance.Gui(cmd.dialogname);
					if (gui == null)
						break;
					gui.gameObject.SetActive(true);
					if (cmd.repeatclick)
						WebSocketClient.Instance.Send(new RequestClickQuestTraceQuestUserCmd_C() { questid = cmd.questid });
				}
				break;
			case ClickQuestTaceEvent.ClickQuestTaceEvent_AttackMonster:
				{
					// TODO: 按照给定的 cmd.npcbaseid 打怪
					var npc = (from i in Npc.All where i.Value.TableInfo.id == cmd.npcbaseid select i.Value).FirstOrDefault();
					if (npc == null)
						break;
					var position = npc.transform.localPosition;
					MainRole.Instance.pathMove.WalkTo(position);
				}
				break;
			default:
				break;
		}
	}
}
