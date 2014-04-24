using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour {

	public UISlider slider;
	public UILabel lable;
	public static string loadedLevelName= "";
	//异步对象   
	private AsyncOperation async;

	// Use this for initialization
	void Start () 
	{
		//在这里开启一个异步任务，   
		//进入loadScene方法。
		slider.value = 0.0f;
		StartCoroutine(loadScene());
	}

	//注意这里返回值一定是 IEnumerator   
	IEnumerator loadScene()   
	{   
		if (Application.loadedLevelName != loadedLevelName) 
		{
			async = Application.LoadLevelAsync (loadedLevelName);
			yield return async;
		}
	}  
	
	// Update is called once per frame
	void Update () 
	{
		if (slider) 
		{
			slider.sliderValue = async.progress;
		}
	}
}
