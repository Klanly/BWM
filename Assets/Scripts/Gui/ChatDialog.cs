using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;
using System.Linq;

public class ChatDialog : MonoBehaviour
{
	public const int MaxChatLines = 10;

	private readonly List<string> lines = new List<string>();

	public GameObject content;
	public UILabel contentChatMessage;

	public UIButton gmCommandButton;

	void Start()
	{
		lines.Clear();
		contentChatMessage.text = string.Empty;

		UIEventListener.Get(gmCommandButton.gameObject).onClick = go => ChatInputBox.SendChat("//help");
	}

	void OnEnable()
	{
		BattleScene.Instance.Gui<ChatSnapshot>().gameObject.SetActive(false);
		NGUITools.BringForward(this.gameObject);
	}

	void OnDisable()
	{
		BattleScene.Instance.Gui<ChatSnapshot>().gameObject.SetActive(true);
	}

	[Execute]
	public static void ShowChat(CommonChatUserCmd_CS cmd)
	{
		if (BattleScene.Instance == null)
		{
			Debug.LogWarning("进场景前收到聊天消息： " + cmd.info);
			return;
		}
		var my = BattleScene.Instance.Gui<ChatDialog>();
		var chat = FormatChatMessage(cmd);
		my.lines.Add(chat);
		if (my.lines.Count > MaxChatLines)
		{
			my.lines.RemoveRange(0, my.lines.Count - MaxChatLines);
			my.contentChatMessage.text = string.Join(string.Empty, my.lines.ToArray());
		}
		else
		{
			my.contentChatMessage.text += chat;
		}

		// 显示缩略聊天消息
		BattleScene.Instance.Gui<ChatSnapshot>().ShowChat(chat);
	}

	/// <summary>
	/// 格式化聊天输出文本，确保换行结尾
	/// </summary>
	/// <remarks>
	/// NGUI Rich Text Manual
	/// http://www.tasharen.com/?page_id=166
	/// </remarks>
	/// <param name="cmd"></param>
	/// <returns></returns>
	private static string FormatChatMessage(CommonChatUserCmd_CS cmd)
	{
		switch(cmd.chatpos)
		{
			case CommonChatUserCmd_CS.ChatPos.ChatPos_Sys:
				return string.Format("[ff0000][GM]{0}[-]\n", cmd.info);
			default:
				return string.Format("{0}: {1}\n", cmd.charname, cmd.info);
		}
	}
}
