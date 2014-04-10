using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class NpcDialog : MonoBehaviour
{
	public UILabel uiTitle;
	public UIButton uiClose;
	public UIScrollView uiScrollView;
	public UIXmlRichText uiXmlRichText;

	void Start()
	{
		UIEventListener.Get(uiClose.gameObject).onClick = go => this.gameObject.SetActive(false);
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

	[Execute]
	public static IEnumerator Execute(ReturnNpcDialogScriptUserCmd_S cmd)
	{
		Npc npc;
		if (Npc.All.TryGetValue(cmd.tempid, out npc) == false)
			yield break;
		var my = BattleScene.Instance.Gui<NpcDialog>();
		my.gameObject.SetActive(true);
		my.SetTitle(npc.TableInfo.name);
		yield return null;
		my.SetMessage(cmd.script);
	}
}
