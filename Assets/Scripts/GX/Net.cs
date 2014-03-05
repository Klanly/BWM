using UnityEngine;
using System.Collections;
using GX;
using GX.Net;
using System.IO;
using System;

public class Net : Singleton<Net>
{
	public const string EchoServer = "ws://echo.websocket.org";

	public MessageDispatcher Dispatcher { get; private set; }
	public GX.Net.WebSocket.State State { get { return WebSocket.Proxy.State; } }

	protected Net()
	{
		this.Dispatcher = new MessageDispatcher();
		this.Dispatcher.StaticRegister();
	}

	public IEnumerator Open(string url)
	{
		WebSocket.Open(url);
		while (true)
		{
			if (this.State != WebSocket.State.Connecting)
				break;
			yield return new WaitForEndOfFrame();
		}
	}

	public void Send(ProtoBuf.IExtensible message)
	{
		WebSocket.Send(message);
	}

	public void Send(params ProtoBuf.IExtensible[] message)
	{
		WebSocket.Send(message);
	}

	void Start()
	{
		StartCoroutine(Dispatch());
	}

	IEnumerator Dispatch()
	{
		while (true)
		{
			yield return null;
			foreach (var msg in WebSocket.Receive())
			{
				IEnumerator coroutine;
				if (Dispatcher.Dispatch(msg, out coroutine) == false)
					Debug.LogWarning(string.Format("未处理的消息: {0}\n{1}", msg.GetType(), msg.ToStringDebug()));
				if (coroutine != null)
				{
					while (coroutine.MoveNext())
						yield return coroutine.Current;
				}
			}
		}
	}
}
