using UnityEngine;
using System.Collections;
using System;
using GX;

public class MessageBox : MonoBehaviour
{
	public UILabel messageBoxTitle;
	public UILabel messageBoxText;
	public GameObject messageBoxOK;
	public GameObject messageBoxCancel;

	protected MessageBox()
	{
	}

	public void Close(GameObject sender = null)
	{
		this.gameObject.SetActive(false);
		UIEventListener.Get(this.messageBoxOK).onClick = null;
		UIEventListener.Get(this.messageBoxCancel).onClick = null;
	}

	public static void Show(string message, UIEventListener.VoidDelegate onOK = null, UIEventListener.VoidDelegate onCancel = null)
	{
		Show(string.Empty, message, onOK, onCancel);
	}

	public static void Show(string title, string message, UIEventListener.VoidDelegate onOK = null, UIEventListener.VoidDelegate onCancel = null)
	{
		var my = GameObject.FindObjectOfType<MessageBox>();
		if (my == null)
		{
			my = (GameObject.Instantiate(Resources.Load("Prefabs/MessageBox")) as GameObject).GetComponent<MessageBox>();
			my.transform.parent = GameObject.Find("UI Root").transform;
			my.transform.localScale = Vector3.one;
		}

		my.gameObject.SetActive(true);

		my.messageBoxTitle.text = title ?? string.Empty;
		my.messageBoxText.text = message;

		onCancel = onCancel ?? my.Close;
		onOK = onOK ?? onCancel;
		UIEventListener.Get(my.messageBoxOK).onClick = onOK;
		UIEventListener.Get(my.messageBoxCancel).onClick = onCancel;
	}
}
