using UnityEngine;
using System.Collections;
using Cmd;

public class ChatSnapshot : MonoBehaviour
{
	public UILabel uiChatMessage;
	public UIButton uiShowChatDialog;

	private string lastChat;

	void Start()
	{
		lastChat = string.Empty;
		uiChatMessage.text = string.Empty;
		UIEventListener.Get(uiShowChatDialog.gameObject).onClick = go => BattleScene.Instance.Gui<ChatDialog>().gameObject.SetActive(true);
	}

	public void ShowChat(string line)
	{
		uiChatMessage.text = line + lastChat;
		lastChat = line;
	}
}
