using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;

public class GuiChatOutput : MonoBehaviour
{
	public const int MaxChatLines = 10;

	public UILabel chatMessage;
	private readonly List<string> lines = new List<string>();

	void Start()
	{
		lines.Clear();
		chatMessage.text = string.Empty;
	}

	[Execute]
	static void ShowChat(CommonChatUserCmd_CS cmd)
	{
		var my = Object.FindObjectOfType<GuiChatOutput>();

		var chat = string.Format("{0}: {1}\n", cmd.charname, cmd.info);
		my.lines.Add(chat);
		if (my.lines.Count > MaxChatLines)
		{
			my.lines.RemoveRange(0, my.lines.Count - MaxChatLines);
			my.chatMessage.text = string.Join(string.Empty, my.lines.ToArray());
		}
		else
		{
			my.chatMessage.text += chat;
		}
	}
}
