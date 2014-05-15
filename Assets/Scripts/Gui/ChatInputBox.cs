using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <summary>
/// </summary>
/// <remarks>ChatInput和NGUI\Examples\Scripts\Other\ChatInput.cs重名</remarks>
public class ChatInputBox : MonoBehaviour
{
	public UIButton sendButton;
	public UIInput chatInput;

	private const int HistorySize = 5;
	private static readonly List<string> history = new List<string>();

	// Use this for initialization
	void Start()
	{
		UIEventListener.Get(sendButton.gameObject).onClick = go => SendChat();
	}

	void Update()
	{
		sendButton.isEnabled = !string.IsNullOrEmpty(chatInput.value.Trim());
		if (sendButton.isEnabled && Input.GetKeyDown(KeyCode.Return))
			SendChat();
	}

	void OnEnable()
	{
		chatInput.isSelected = true;
	}

	/// <summary>
	/// 设置聊天输入框中的文字
	/// </summary>
	/// <param name="text"></param>
	public void SetText(string text)
	{
		chatInput.value = text;
		BattleScene.Instance.Gui<ChatDialog>().gameObject.SetActive(true);
	}

	private void SendChat()
	{
		var str = chatInput.value.Trim(); 
		SendChat(str);
		chatInput.value = string.Empty;
		chatInput.isSelected = true;

		// 更新发送历史
		history.Remove(str);
		history.Add(str);
		if (history.Count > HistorySize)
			history.RemoveRange(0, history.Count - HistorySize);
	}

	public static void SendChat(string message)
	{
		ProtoBuf.IExtensible cmd = null;
		if (cmd == null)
			cmd = CreateGmCommand(message);
		if (cmd == null)
			cmd = CreateCommonChat(message);
		Net.Instance.Send(cmd);
	}

	private static Regex gmcommandRegex = new Regex(@"^\s*//\s*(?<method>\w+)(\s+(?<params>.*?)\s*)?$");
	private static GMCommandChatUserCmd_C CreateGmCommand(string message)
	{
		if (string.IsNullOrEmpty(message))
			return null;
		var match = gmcommandRegex.Match(message);
		if(match.Success == false)
			return null;
		return new GMCommandChatUserCmd_C()
		{
			method = match.Groups["method"].Value,
			@params = match.Groups["params"].Value,
		};
	}

	private static CommonChatUserCmd_CS CreateCommonChat(string message)
	{
		return new CommonChatUserCmd_CS()
		{
			chatpos = CommonChatUserCmd_CS.ChatPos.ChatPos_Normal,
			info = NGUIText.StripSymbols(message),
		};
	}

}
