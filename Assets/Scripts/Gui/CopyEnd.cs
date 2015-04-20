using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class CopyEnd : MonoBehaviour
{

	public GameObject uiClose;
	private PrepairFinishStageQuestUserCmd_S serverInfo;

	// Use this for initialization
	void Start()
	{
		BattleScene.AddGuiToTop(gameObject);

		// 关闭按钮
		UIEventListener.Get(uiClose).onClick = OnGetPresent;
	}
	public void SetStage(PrepairFinishStageQuestUserCmd_S cmd)
	{
		serverInfo = cmd;
	}
	public void OnGetPresent(GameObject sender = null)
	{
		WebSocketClient.Instance.Send(new RequestFinishStageQuestUserCmd_C()
		{
			stageid = serverInfo.stageid,
		});
		//this.transform.parent.gameObject.SetActive(false);
		this.gameObject.SetActive(false);
	}
	#region 网络消息处理
	[Execute]
	public static void Execute(PrepairFinishStageQuestUserCmd_S cmd)
	{
		var my = BattleScene.Instance.Gui<CopyEnd>();
		my.gameObject.SetActive(true);
		my.SetStage(cmd);
	}
	#endregion
}
