using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

/// <summary>
/// 任务交接界面
/// </summary>
public class QuestDialog : MonoBehaviour
{
	public const string UriSchemeIndex = "index://";

	public UILabel uiTitle;
	public UIScrollView uiScrollView;
	public UIXmlRichText uiXmlRichText;
	public UIButton uiOKButton;

	private uint questID;
	private QuestProcess questState;

	void Start()
	{
		UIEventListener.Get(uiOKButton.gameObject).onClick = go =>
		{
			if (questState == QuestProcess.QuestProcess_CanDone)
			{
				Net.Instance.Send(new RequestFinishQuestQuestUserCmd_C()
				{
					questid = questID,
				});
			}
			else
			{
				Net.Instance.Send(new RequestAcceptQuestQuestUserCmd_C()
				{
					questid = questID,
				});
			}

			this.GetComponent<Closeable>().Close();
		};
		uiXmlRichText.UrlClicked += OnUrlClicked;
	}

	public void Present(uint questID, QuestProcess state, string xml)
	{
		if (state != QuestProcess.QuestProcess_CanDone && state != QuestProcess.QuestProcess_None)
		{
			Debug.LogError(string.Format("不可接受的任务状态: {0} {1}", questID, state));
			return;
		}
		this.questID = questID;
		this.questState = state;
		uiOKButton.GetComponentInChildren<UILabel>().text = this.questState == QuestProcess.QuestProcess_CanDone ? "完成" : "接受";
		uiTitle.text = "任务交接";
		uiXmlRichText.Clear();
		uiXmlRichText.AddXml(xml);
	}

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}

	private void OnUrlClicked(UIWidget sender, string url)
	{
		Debug.Log(string.Format("OnUrlClicked: {0}, {1}", sender.name, url));
	}
}
