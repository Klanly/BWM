using UnityEngine;
using System.Collections;
using GX;
using GX.Net;
using System.IO;

public class Net : Singleton<Net>
{
	private WebSocket socket;
	public MessageDispatcher Dispatcher { get; private set; }

	protected Net()
	{
		this.socket = new WebSocket();
		this.Dispatcher = new MessageDispatcher();
		this.Dispatcher.StaticRegister();
	}

	public void Open(string url = "ws://echo.websocket.org")
	{
		socket.Open(url);
	}

	public void Send(ProtoBuf.IExtensible message)
	{
		socket.Send(message);
	}

	public void Send(params ProtoBuf.IExtensible[] message)
	{
		socket.Send(message);
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
			foreach (var msg in socket.Receive())
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
