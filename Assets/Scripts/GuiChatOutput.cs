using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;
using System.Linq;

public class GuiChatOutput : MonoBehaviour
{
	public const int MaxChatLines = 10;
	public const int MaxSnapshotLines = 2;

	private readonly List<string> lines = new List<string>();

	public GameObject content;
	public UILabel contentChatMessage;
	public UIButton contentToggle;

	public GameObject snapshot;
	public UILabel snapshotChatMessage;
	public UIButton snapshotToggle;

	void Start()
	{
		lines.Clear();
		contentChatMessage.text = string.Empty;
		UIEventListener.Get(contentToggle.gameObject).onClick = Toggle;

		snapshotChatMessage.text = string.Empty;
		UIEventListener.Get(snapshotToggle.gameObject).onClick = Toggle;

		Toggle();
	}

	void Toggle(GameObject sender = null)
	{
		var full = content.activeSelf;
		content.SetActive(!full);
		snapshot.SetActive(full);

		NGUITools.BringForward(this.gameObject);
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
			my.contentChatMessage.text = string.Join(string.Empty, my.lines.ToArray());
		}
		else
		{
			my.contentChatMessage.text += chat;
		}

		my.snapshotChatMessage.text = string.Join(string.Empty, my.lines.Skip(my.lines.Count - MaxSnapshotLines).ToArray());
	}
}
