using UnityEngine;
using System.Collections;

public class CopyStart : MonoBehaviour {

	public GameObject uiBack;

	// Use this for initialization
	void Start () 
	{
		BattleScene.AddToPanel(gameObject);

		uiBack.GetComponent<TweenHeight>().AddOnFinished(onFinish);
	}

	void onFinish()
	{
		Destroy(gameObject);

		// 打开CopyEnd界面
		Instantiate(Resources.Load("Prefabs/Gui/CopyEnd"));
	}
}
