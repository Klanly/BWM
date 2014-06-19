using UnityEngine;
using System.Collections;

public class CopyEnd : MonoBehaviour {

	public GameObject uiClose;

	// Use this for initialization
	void Start () 
	{
		BattleScene.AddGuiToTop(gameObject);

		// 关闭按钮
		UIEventListener.Get(uiClose).onClick = go => Destroy(transform.parent.gameObject);
	}
}
