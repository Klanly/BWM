using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using System.Text.RegularExpressions;

/// <summary>
/// </summary>
/// <remarks>ChatInput和NGUI\Examples\Scripts\Other\ChatInput.cs重名</remarks>
public class GXChatInput : MonoBehaviour
{
	public UIButton sendButton;
	public UIInput chatInput;

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

	private void SendChat()
	{
		SendChat(chatInput.value.Trim());
		chatInput.value = string.Empty;
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
			info = message,
		};
	}

}
