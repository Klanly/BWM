using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class QuestDialog : MonoBehaviour
{
	public const string UriSchemeIndex = "index://";

	public UILabel uiTitle;
	public UIScrollView uiScrollView;
	public UIXmlRichText uiXmlRichText;

	public ulong tempid { get; private set; }
	public string token { get; private set; }

	void Start()
	{
		uiXmlRichText.UrlClicked += OnUrlClicked;
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
