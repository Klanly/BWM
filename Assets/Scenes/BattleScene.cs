using UnityEngine;
using System.Collections;

public class BattleScene : MonoBehaviour
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
