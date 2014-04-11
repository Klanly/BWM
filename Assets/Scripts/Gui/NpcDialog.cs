using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class NpcDialog : MonoBehaviour
{
	public const string UriSchemeIndex = "index://";

	public UILabel uiTitle;
	public UIButton uiClose;
	public UIScrollView uiScrollView;
	public UIXmlRichText uiXmlRichText;

	public ulong tempid { get; private set; }
	public string token { get; private set; }

	void Start()
	{
		UIEventListener.Get(uiClose.gameObject).onClick = Close;
		uiXmlRichText.UrlClicked += OnUrlClicked;
	}

	public void Close(GameObject sender = null)
	{
		this.gameObject.SetActive(false);
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
		if (url.StartsWith(UriSchemeIndex))
		{
			ulong index;
			if (ulong.TryParse(url.Substring(UriSchemeIndex.Length), out index))
			{
				Net.Instance.Send(new SelectNpcDialogScriptUserCmd_C()
				{
					tempid = this.tempid,
					token = this.token,
					index = index,
				});
				Close();
			}
		}
	}

	[Execute]
	public static IEnumerator Execute(ReturnNpcDialogScriptUserCmd_S cmd)
	{
		Npc npc;
		if (Npc.All.TryGetValue(cmd.tempid, out npc) == false)
			yield break;
		var my = BattleScene.Instance.Gui<NpcDialog>();
		my.tempid = cmd.tempid;
		my.token = cmd.token;
		my.gameObject.SetActive(true);
		my.SetTitle(npc.TableInfo.name);
		yield return null;
		my.SetMessage(cmd.script);
	}
}
