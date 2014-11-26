using UnityEngine;
using System.Collections;

/// <summary>
/// 键盘Escape退出游戏
/// </summary>
public class EscapeQuit : MonoBehaviour
{
	public string lastSceneName;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (string.IsNullOrEmpty(lastSceneName))
				ExitGame();
			else
				Application.LoadLevel(lastSceneName);
		}
	}

	/// <summary>
	/// 能同时适应于真机和编辑器的退出游戏
	/// </summary>
	public static void ExitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
