using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;
using System.Linq;

public class GXChatOutput : MonoBehaviour
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

		Toggle(true);
	}

	void Toggle(GameObject sender)
	{
		Toggle(content.activeSelf);
	}

	public void Toggle(bool toMinichat)
	{
		content.SetActive(!toMinichat);
		snapshot.SetActive(toMinichat);

		NGUITools.BringForward(this.gameObject);

		var input = BattleScene.Instance.Gui<GXChatInput>().gameObject;
		input.SetActive(!toMinichat);
		NGUITools.BringForward(input);
	}

	[Execute]
	static void ShowChat(CommonChatUserCmd_CS cmd)
	{
		var my = Object.FindObjectOfType<GXChatOutput>();

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
