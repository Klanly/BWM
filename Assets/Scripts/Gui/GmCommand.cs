using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;
using System.Linq;

public class GmCommand : MonoBehaviour
{
	public GameObject itemTemplate;
	public UIScrollView scrollView;

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}

	void SetValues(IEnumerable<GMHelpInfo> infos)
	{
		var canvas = scrollView.GetComponentInChildren<UIGrid>();
		// remove all childer of canvas
		foreach (Transform d in canvas.transform)
			DestroyObject(d.gameObject);
		foreach (var i in infos)
		{
			var info = i;
			var item = GameObject.Instantiate(itemTemplate) as GameObject;
			item.transform.parent = canvas.transform;
			item.transform.localScale = Vector3.one;

			item.GetComponentInChildren<UILabel>().text = info.method;
			UIEventListener.Get(item.GetComponentInChildren<UIButton>().gameObject).onClick = go =>
			{
				Debug.Log(info.Dump());
				BattleScene.Instance.Gui<ChatInputBox>().SetText(string.Format("//{0} {1}", info.method, info.example));
			};
		}

		canvas.gameObject.SetActive(true);
		canvas.Reposition();
		scrollView.ResetPosition();
	}

	[Execute]
	public static IEnumerator Execute(GMCommandListChatUserCmd_S cmd)
	{
		var my = BattleScene.Instance.Gui<GmCommand>();
		my.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		my.SetValues(cmd.list.OrderBy(i => i.method));
	}
}
