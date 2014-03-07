using UnityEngine;
using System.Collections;

/// <summary>
/// 编辑器下自动跳转到LoginScene，方便开发
/// </summary>
public class AutoLoginScene : MonoBehaviour
{
	void Start()
	{
#if UNITY_EDITOR
		if (System.IO.Path.GetFileNameWithoutExtension(UnityEditor.EditorApplication.currentScene) != LoginScene.Name)
		{
			Application.LoadLevel(LoginScene.Name);
			return;
		}
#endif
	}
}
