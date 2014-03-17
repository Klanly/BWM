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
		var canvas = scrollView.GetComponent<UIGrid>();

		// remove all childer of canvas
		foreach (Transform d in canvas.transform)
			DestroyObject(d.gameObject);
		canvas.Reposition();
		scrollView.ResetPosition();

		foreach (var i in infos)
		{
			var info = i;
			var item = GameObject.Instantiate(itemTemplate) as GameObject;
			item.transform.parent = canvas.transform;
			item.transform.localScale = Vector3.one;

			item.GetComponentInChildren<UILabel>().text = info.method;
			UIEventListener.Get(item.GetComponentInChildren<UIButton>().gameObject).onClick = go =>
			{
				Debug.Log(info.ToStringDebug());
				BattleScene.Instance.Gui<GXChatInput>().chatInput.value = string.Format("// {0} {1}", info.method, info.example);
			};
		}

		canvas.Reposition();
	}

	[Execute]
	static void Execute(GMCommandListChatUserCmd_S cmd)
	{
		var my = BattleScene.Instance.Gui<GXGmCommand>();
		my.SetValues(cmd.list);
	}
}
