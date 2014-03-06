using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class GuiChatInput : MonoBehaviour
{
	public UIButton sendButton;
	public UIInput chatInput;

	// Use this for initialization
	void Start()
	{
		UIEventListener.Get(sendButton.gameObject).onClick = go => SendChat();
	}

	public void SendChat(string message = null)
	{
		if (message == null)
		{
			message = chatInput.value.Trim();
			chatInput.value = null;
		}

		Net.Instance.Send(new CommonChatUserCmd_CS()
		{
			charid = MainCharacter.ServerInfo.data.charid,
			chatpos = CommonChatUserCmd_CS.ChatPos.ChatPos_Normal,
			info = message,
		});
	}

	void Update()
	{
		sendButton.isEnabled = !string.IsNullOrEmpty(chatInput.value.Trim());
		if (sendButton.isEnabled && Input.GetKeyDown(KeyCode.Return))
			SendChat();
	}
}
