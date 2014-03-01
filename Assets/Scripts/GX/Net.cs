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
	}

	public void Open(string url = "ws://echo.websocket.org")
	{
		StopCoroutine("Run");
		socket.Open(url);
		StartCoroutine("Run");
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

	IEnumerator Run()
	{
		return socket.Run();
	}

	IEnumerator Dispatch()
	{
		while (true)
		{
			var buf = socket.Receive();
			if (buf == null)
			{
				yield return null;
				continue;
			}
			foreach (var msg in socket.Receive())
			{
				if (Dispatcher.Dispatch(msg) == false)
					Debug.Log(string.Format("未处理的消息: {0}\n{1}", msg.GetType(), msg.ToStringDebug()));
				yield return msg;
			}
			yield return null;
		}
		throw new System.NotImplementedException();
	}
}
