using UnityEngine;
using System.Collections;

/// <summary>
/// 键盘Escape退出游戏
/// </summary>
public class EscapeQuit : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
