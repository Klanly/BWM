using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Cmd;
using GX;
using GX.Net;

public class Revive : MonoBehaviour
{
	public UILabel title;
	public UILabel text;
	public GameObject OK;

	void Start()
	{
		BattleScene.AddGuiToTop(gameObject);
		UIEventListener.Get(OK).onClick = go =>
		{
			Net.Instance.Send(new RequestReliveUserCmd_C()
			{
				type = 1,
			});
		};
	}

	/// <summary>
	/// 打开复活界面
	/// </summary>
	/// <param name="cmd"></param>
	/// <returns></returns>
	[Execute]
	public static void Execute(ReliveMethodsReliveUserCmd_S cmd)
	{
		// 加载复活界面
		var gui = Instantiate(Resources.Load("Prefabs/Gui/revive")) as GameObject;
		var revive = gui.GetComponent<Revive>();
		revive.title.text = "复活";
		revive.text.text = "立即复活";
	}

	/// <summary>
	/// 复活
	/// </summary>
	/// <param name="cmd"></param>
	/// <returns></returns>
	[Execute]
	public static void Execute(ReturnReliveUserCmd_S cmd)
	{
		NGUITools.Destroy(FindObjectOfType<Revive>().transform.parent.gameObject);
		if (MainRole.Instance)
			MainRole.Instance.animator.Play("Ani_Stand");
	}
}
