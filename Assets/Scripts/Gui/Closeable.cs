using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Closeable : MonoBehaviour
{
	private static readonly List<Closeable> s_tracker = new List<Closeable>();
	public static Closeable TopMost()
	{
		return s_tracker.OrderByDescending(i => i.transform, new UIDepthComparer()).FirstOrDefault();
	}

	public UIButton closeButton;

	void Start()
	{
		UIEventListener.Get(closeButton.gameObject).onClick = Close;
	}

	void OnEnable()
	{
		s_tracker.Add(this);
		NGUITools.BringForward(closeButton.gameObject);
	}

	void OnDisable()
	{
		s_tracker.Remove(this);
	}

	public void Close(GameObject sender = null)
	{
		this.gameObject.SetActive(false);
	}
}
