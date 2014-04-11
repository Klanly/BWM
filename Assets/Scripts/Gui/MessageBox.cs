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
		NGUITools.Destroy(this.gameObject);
	}

	public static MessageBox Show(string message, UIEventListener.VoidDelegate onOK = null, UIEventListener.VoidDelegate onCancel = null)
	{
		return Show(string.Empty, message, onOK, onCancel);
	}

	public static MessageBox Show(string title, string message, UIEventListener.VoidDelegate onOK = null, UIEventListener.VoidDelegate onCancel = null)
	{
		var root = GameObject.Find("UI Root");
		var my = NGUITools.AddChild(root, (GameObject)Resources.Load("Prefabs/Gui/MessageBox")).GetComponent<MessageBox>();
		my.GetComponent<UIWidget>().SetAnchor(root, 0, 0, 0, 0);
		NGUITools.BringForward(my.gameObject);

		my.messageBoxTitle.text = title ?? string.Empty;
		my.messageBoxText.text = message;

		onCancel = onCancel ?? my.Close;
		onOK = onOK ?? onCancel;
		UIEventListener.Get(my.messageBoxOK).onClick = onOK;
		UIEventListener.Get(my.messageBoxCancel).onClick = onCancel;

		return my;
	}
}
