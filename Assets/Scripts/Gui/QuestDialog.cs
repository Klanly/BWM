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

	public string QuestDetail { get; set; }
	public uint QuestID { get; set; }

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		uiOKButton.GetComponentInChildren<UILabel>().text = QuestManager.Instance[QuestID].squest.Finished ? "完成" : "接受";
		UIEventListener.Get(uiOKButton.gameObject).onClick = go =>
		{
			if (QuestManager.Instance[QuestID].squest.Finished)
			{
				Net.Instance.Send(new RequestFinishQuestQuestUserCmd_CS()
				{
					questid = QuestID,
				});
			}
			else
			{
				Net.Instance.Send(new RequestAcceptQuestQuestUserCmd_CS()
				{
					questid = QuestID,
				});
			}

			this.GetComponent<Closeable>().Close();
		};

		uiXmlRichText.UrlClicked += OnUrlClicked;
		SetTitle("任务交接");
		SetMessage(QuestDetail);
	}

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}

	public void SetTitle(string title)
	{
		uiTitle.text = title;
	}

	public void SetMessage(string xml)
	{
		uiXmlRichText.Clear();
		uiXmlRichText.AddXml(xml);
	}

	private void OnUrlClicked(UIWidget sender, string url)
	{
		Debug.Log(string.Format("OnUrlClicked: {0}, {1}", sender.name, url));
	}
}
