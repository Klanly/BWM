using UnityEngine;
using System.Collections;

/// <summary>
/// 编辑器下自动跳转到LoginScene，方便开发
/// </summary>
public class AutoLoginScene : MonoBehaviour
{
#if UNITY_EDITOR
	static bool jumped = false;
	void Start()
	{
		if (jumped)
			return;
		jumped = true;
		var current = System.IO.Path.GetFileNameWithoutExtension(UnityEditor.EditorApplication.currentScene);
		if (current != LoginScene.Name)
		{
			Debug.Log(string.Format("Auto jump: {0} -> {1}", current, LoginScene.Name));
			Application.LoadLevel(LoginScene.Name);
		}
	}
#endif
}
