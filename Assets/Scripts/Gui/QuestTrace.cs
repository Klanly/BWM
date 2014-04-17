using UnityEngine;
using System.Collections;
using System.Linq;
using Cmd;

/// <summary>
/// 任务追踪界面
/// </summary>
public class QuestTrace : MonoBehaviour
{
	public UIXmlRichText uiXmlRichText;
	public GameObject uiBackground;

	// Use this for initialization
	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		QuestManager.Instance.Changed += OnQuestChanged;
		OnQuestChanged(QuestManager.Instance);
		uiXmlRichText.UrlClicked += OnQuestTraceLinkClicked;
	}

	void OnDestroy()
	{
		QuestManager.Instance.Changed -= OnQuestChanged;
	}

	private void OnQuestChanged(QuestManager quests)
	{
		uiXmlRichText.gameObject.SetActive(quests.Any());
		uiBackground.SetActive(uiXmlRichText.gameObject.activeSelf);

		uiXmlRichText.Clear();
		uiXmlRichText.AddXml(string.Join("\n", quests.Select(i => i.TraceContent).ToArray()));
	}

	private void OnQuestTraceLinkClicked(UIWidget sender, string href)
	{
		Debug.Log(href);
		Net.Instance.Send(new RequestQuestDetailInfoQuestUserCmd_C()
		{
			questid = 1000,
		});
	}
}
