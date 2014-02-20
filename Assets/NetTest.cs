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
		socket.Open();
		StartCoroutine(socket.Run());

		socket.Send(new Cmd.Login.VersionVerify() { version = "1.0" });
		socket.Send(new Cmd.Login.VersionVerify() { version = "2.0" });
		socket.Send(new Cmd.Login.VersionVerify() { version = "3.0" }, new Cmd.Login.VersionVerify() { version = "4.0" });
	}

	void Update()
	{
		foreach (var cmd in socket.Receive())
		{
			var vv = cmd as VersionVerify;
			Debug.Log(vv.version);
			//yield return vv;
		}
	}
}
