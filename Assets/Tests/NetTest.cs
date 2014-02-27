using UnityEngine;
using System.Collections;
using GX.Net;
using System.IO;
using Cmd.Login;

public class NetTest : MonoBehaviour
{
	public MessageSerializer Serializer { get; private set; }
	private WebSocket socket;

	// Use this for initialization
	IEnumerator Start()
	{
		socket = new WebSocket();
		yield return null;
		socket.Open("ws://192.168.85.71:8000/shen/user");
		StartCoroutine(socket.Run());

		socket.Send(new Cmd.Login.VersionVerify_CS() { version = 1 });
		socket.Send(new Cmd.Login.VersionVerify_CS() { version = 2 });
		socket.Send(new Cmd.Login.VersionVerify_CS() { version = 3 }, new Cmd.Login.VersionVerify_CS() { version = 4 });
	}

	void Update()
	{
		foreach (var cmd in socket.Receive())
		{
			var vv = cmd as VersionVerify_CS;
			Debug.Log(vv.version);
			//yield return vv;
		}
	}
}
