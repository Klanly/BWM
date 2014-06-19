using UnityEngine;
using System.Collections;

public class CopyEnd : MonoBehaviour {

	public GameObject uiClose;

	// Use this for initialization
	void Start () 
	{
		BattleScene.AddToPanel(gameObject);

		// 关闭按钮
		UIEventListener.Get(uiClose).onClick = go => Destroy(gameObject);
	}
}
