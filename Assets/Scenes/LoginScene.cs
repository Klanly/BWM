using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd.Login;

public class LoginScene : MonoBehaviour
{
	const int Version = 2014;
	const int GameID = 100;

	public UIGrid zoneList;
	public GameObject zoneButton;

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

		socket.Send(new Cmd.Login.AccountTokenVerify_CS() { version = Version, gameid = GameID, account = "1024", token = "1" });
	}

	void OnDestroy()
	{
		socket.Dispatcher.UnRegister(this);
		Debug.Log(socket.Dispatcher);
	}

	[Execute]
	void Execute(AccountTokenVerify_CS cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
		//socket.Send(new UserLoginRequest_C() { username = "1024", gameversion = cmd.version, gameid = cmd.gameid, zoneid = 101 });
	}

	[Execute]
	void Execute(ZoneInfoList_S cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
		foreach (Transform t in zoneList.transform)
			DestroyObject(t.gameObject);
		zoneList.Reposition();

		foreach (var zone in cmd.server)
		{
			var item = Instantiate(zoneButton) as GameObject;
			item.transform.parent = zoneList.transform;
			item.transform.localScale = Vector3.one;
			item.GetComponentInChildren<UILabel>().text = zone.zonename;
			switch(zone.state)
			{
				case ServerState.Normal:
					var zoneid = zone.zoneid;
					UIEventListener.Get(item).onClick = go => socket.Send(new UserLoginRequest_C() { gameversion = Version, gameid = cmd.gameid, zoneid = zoneid });
					break;
				case ServerState.Shutdown:
					var button = item.GetComponentInChildren<UIButton>();
					button.enabled= false;
					button.UpdateColor(false, false);
					break;
				default:
					throw new System.NotImplementedException();
			}
		}
		zoneList.Reposition();
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
		socket.Send(new UserLoginToken_C() { logintempid = rev.logintempid, accountid = rev.accountid });
	}
}
