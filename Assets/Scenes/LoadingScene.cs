using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
	public UISlider slider;
	public static string loadedLevelName = "";
	private AsyncOperation async;

	void Start()
	{
		slider.value = 0.0f;
		StartCoroutine(LoadLevel());
	}

	IEnumerator LoadLevel()
	{
		if (Application.loadedLevelName != loadedLevelName)
		{
			async = Application.LoadLevelAsync(loadedLevelName);
			yield return async;
		}
	}

	void Update()
	{
		slider.value = async.progress;
	}
}