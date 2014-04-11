using UnityEngine;
using System.Collections;

public class Closeable : MonoBehaviour
{
	public UIButton closeButton;

	void Start()
	{
		UIEventListener.Get(closeButton.gameObject).onClick = Close;
	}

	void OnEnable()
	{
		NGUITools.BringForward(closeButton.gameObject);
	}
	
	void Update()
	{
	}

	public void Close(GameObject sender = null)
	{
		this.gameObject.SetActive(false);
	}
}
