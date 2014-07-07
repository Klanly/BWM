using Cmd;
using GX.Net;
using UnityEngine;
using System.Collections;

public class CopyStart : MonoBehaviour
{

	public GameObject uiBack;

	// Use this for initialization
	void Start()
	{
		BattleScene.AddGuiToTop(gameObject, false);

		uiBack.GetComponent<TweenHeight>().AddOnFinished(onFinish);
	}

	void onFinish()
	{
		Destroy(transform.parent.gameObject);

	}
	#region 网络消息处理
	[Execute]
	public static void Execute(ReturnOpenStageQuestUserCmd_S cmd)
	{
		//NGUITools.Destroy(FindObjectOfType<CopyEnter>().transform.parent.gameObject);
		//NGUITools.Destroy(FindObjectOfType<CopyEnter2>().transform.parent.gameObject);
		// 打开copystart界面
		UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Gui/CopyStart"));
	}
	#endregion
}
