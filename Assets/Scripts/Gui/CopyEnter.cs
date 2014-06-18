using UnityEngine;
using System.Collections;

public class CopyEnter : MonoBehaviour {

	public GameObject uiClose;

	// Use this for initialization
	void Start () 
	{
		BattleScene.AddToPanel(gameObject);
		UIEventListener.Get(uiClose).onClick = go => Destroy(gameObject);
	}
}
