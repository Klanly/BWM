using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;

public class GXGmCommand : MonoBehaviour
{
	public GameObject itemTemplate;
	public UIScrollView scrollView;

	void SetValues(IEnumerable<GMHelpInfo> infos)
	{
		foreach (var i in infos)
		{
			Debug.LogError(i.method);
		}
	}

	[Execute]
	static void Execute(GMCommandListChatUserCmd_S cmd)
	{
		var my = BattleScene.Instance.Gui<GXGmCommand>();
		my.SetValues(cmd.list);
	}
}
