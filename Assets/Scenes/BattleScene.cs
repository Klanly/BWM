using UnityEngine;
using System.Collections;

public class BattleScene : MonoBehaviour
{
	public GuiChatInput guiChatInput;
	public GuiChatOutput guiChatOutput;

	void Start()
	{
		guiChatInput.gameObject.SetActive(true);
		guiChatOutput.gameObject.SetActive(true);
	}
}
