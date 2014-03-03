using UnityEngine;
using System.Collections;
using System;

public class MessageBox : MonoBehaviour
{
	private string title;
	private string message;

	public UILabel messageBoxTitle;
	public UILabel messageBoxText;
	public GameObject messageBoxOK;
	public GameObject messageBoxCancel;

	void Start()
	{
		Close();
	}

	public void Close(GameObject sender = null)
	{
		this.gameObject.SetActive(false);
		UIEventListener.Get(this.messageBoxOK).onClick = null;
		UIEventListener.Get(this.messageBoxCancel).onClick = null;
	}

	public void Show(string message, string title = null, UIEventListener.VoidDelegate onOK = null, UIEventListener.VoidDelegate onCancel = null)
	{
		this.gameObject.SetActive(true);

		this.title = title ?? string.Empty;
		this.message = message;
		
		onCancel = onCancel ?? Close;
		onOK = onOK ?? onCancel;
		UIEventListener.Get(this.messageBoxOK).onClick = onOK;
		UIEventListener.Get(this.messageBoxCancel).onClick = onCancel;
	}

	void Update()
	{
		this.messageBoxTitle.text = title ?? string.Empty;
		this.messageBoxText.text = message ?? string.Empty;
	}
}
