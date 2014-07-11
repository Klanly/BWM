using UnityEngine;
using System.Collections;

public class RichTextEditor : MonoBehaviour
{
	public NpcDialog npcDialog;
	private string value = string.Empty;

	void Start()
	{
		npcDialog.gameObject.SetActive(true);
		npcDialog.transform.localScale = Vector3.one;
		npcDialog.transform.position = Vector3.zero;
		npcDialog.SetTitle("富文本编辑器");
	}

	void OnGUI()
	{
		value = GUI.TextArea(new Rect(Screen.width * 0.5f + 10, 10, Screen.width * 0.5f - 20, Screen.height - 50), value);
		if (GUI.Button(new Rect(Screen.width * 0.75f - 40, Screen.height - 30, 80, 20), "TEST"))
			Run();
	}

	private void Run()
	{
		try
		{
			Debug.Log(value);
			npcDialog.SetMessage(value);
		}
		catch (System.Exception ex)
		{
			Debug.LogError("错误: " + ex.Message);
		}
	}
}
