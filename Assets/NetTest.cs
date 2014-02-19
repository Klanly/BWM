using UnityEngine;
using System.Collections;
using GX.Net;
using System.IO;
using Cmd.Login;

public class NetTest : MonoBehaviour
{
	public MessageSerializer Serializer { get; private set; }

	// Use this for initialization
	IEnumerator Start()
	{
		yield return null;

		Serializer = new MessageSerializer();
		Debug.Log(Serializer);

		var ws = new HTTP.WebSocket();
		StartCoroutine(ws.Dispatcher());

		Debug.Log(ws);

		ws.Connect("http://echo.websocket.org");

		ws.OnTextMessageRecv += (e) =>
		{
			Debug.Log("Reply came from server -> " + e);
		};
		//ws.Send("Hello");

		//ws.Send("Hello again!");

		//ws.Send("Goodbye");

		ws.OnBinaryMessageRecv += e =>
		{
			using(var mem = new MemoryStream(e))
			{
				print(string.Format("{0}/{1}", mem.Position, mem.Length));
				while (mem.Position < mem.Length)
				{
					var msg = Serializer.Deserialize(mem);
					if (msg is VersionVerify)
						Debug.Log((msg as VersionVerify).version);
					else
						Debug.Log(msg);
					print(string.Format("{0}/{1}", mem.Position, mem.Length));
				}
				print(string.Format("{0}/{1}", mem.Position, mem.Length));
			}
		};

		using (var mem = new MemoryStream())
		{
			Serializer.Serialize(new Cmd.Login.VersionVerify() { version = "1.0" }, mem);
			Debug.Log(System.BitConverter.ToString(mem.ToArray()));
			Serializer.Serialize(new Cmd.Login.VersionVerify() { version = "2.0" }, mem);
			Debug.Log(System.BitConverter.ToString(mem.ToArray()));
			ws.Send(mem.ToArray());
		}

	}

	// Update is called once per frame
	void Update()
	{

	}
}
