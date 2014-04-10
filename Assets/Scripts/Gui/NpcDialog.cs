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

	[Execute]
	public static IEnumerator Execute(ReturnNpcDialogScriptUserCmd_S cmd)
	{
		Npc npc;
		if (Npc.All.TryGetValue(cmd.tempid, out npc) == false)
			yield break;
		cmd.script = "<a href='lua://g_gmCmd?id=1001&amp;num=5'>来5个id为1001的道具</a>";
		var my = BattleScene.Instance.Gui<NpcDialog>();
		my.gameObject.SetActive(true);
		my.SetTitle(npc.TableInfo.name);
		yield return null;
		my.SetMessage(cmd.script);
	}
}
