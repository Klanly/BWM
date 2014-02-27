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
		socket.Dispatcher.Register(this);
		Debug.Log(socket.Dispatcher);

		yield return null;
		socket.Open("ws://192.168.85.71:7000/shen/user");
		StartCoroutine(socket.Run());
		StartCoroutine(socket.Dispatch());

		socket.Send(new Cmd.Login.VersionVerify_CS() { version = 2014, gameid = 100 });
	}

	void OnDestroy()
	{
		socket.Dispatcher.UnRegister(this);
		Debug.Log(socket.Dispatcher);
	}

	[Execute]
	void Execute(VersionVerify_CS cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
		socket.Send(new UserLoginRequest_C() { username = "1024", gameversion = cmd.version, gameid = cmd.gameid, zoneid = 101});
	}

	[Execute]
	void Execute(ZoneInfoList_S cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
	}
	
	[Execute]
	void Execute(UserLoginRequest_C cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
	}

	[Execute]
	void Execute(UserLoginReturnFail_S cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
	}

	[Execute]
	void Execute(UserLoginReturnOk_S cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
		var rev = cmd as UserLoginReturnOk_S;
		socket.Open(rev.gatewayurl);
		socket.Send(new UserLoginToken_C() { logintempid = rev.logintempid, userid = rev.userid });
	}
}
