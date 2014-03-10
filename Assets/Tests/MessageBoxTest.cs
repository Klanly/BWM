using UnityEngine;
using System.Collections;
using System;

public class MessageBoxTest : MonoBehaviour
{
	public GameObject modelTest;
	public GameObject showMessagebox;

	private int index = 1;

	// Use this for initialization
	void Start()
	{
		UIEventListener.Get(this.modelTest).onClick = go => Debug.LogError("This can't be click when any messagebox is open");
		UIEventListener.Get(this.showMessagebox).onClick = go =>
		{
			MessageBox.Show(index++.ToString(), DateTime.Now.ToString());
			MessageBox.Show(index++.ToString(), DateTime.Now.ToString());
			MessageBox.Show(index++.ToString(), DateTime.Now.ToString());
		};
		Debug.Log("Ready");
	}
}
