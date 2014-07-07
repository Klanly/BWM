using Cmd;
using GX.Net;
using UnityEngine;
using System.Collections;

public class CopyEnter2 : MonoBehaviour
{

	public GameObject uiClose;
	public GameObject[] uiCopys;
	public GameObject uiEnter;
	private int selected = 0;

	// Use this for initialization
	void Start()
	{
		BattleScene.AddGuiToTop(gameObject);

		// 关闭按钮
		UIEventListener.Get(uiClose).onClick = go => Destroy(transform.parent.gameObject);

		// 每个副本按钮点击事件
		for (int i = 0; i < uiCopys.Length; ++i)
		{
			var index = i;
			UIEventListener.Get(uiCopys[i]).onClick = go =>
			{
				Debug.Log("copy" + index);
				foreach (var t in uiCopys)
					t.GetComponent<UIButton>().enabled = true;
				uiCopys[index].GetComponent<UIButton>().enabled = false;
				selected = index;
			};
		}

		// 进入按钮
		UIEventListener.Get(uiEnter).onClick = go =>
		{
			Debug.Log("select copy:" + selected);
			Net.Instance.Send(new RequestOpenStageQuestUserCmd_C() { stageid = (uint)selected });


		};
	}

	public void SetDefaultSelect(uint stageid)
	{
		selected = (int)stageid;
	}

	#region 网络消息处理
	#endregion

}
