using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd.Login;
using System;

public class LoginScene : MonoBehaviour
{
	public const int Version = 2014;
	public const int GameID = 100;

	public UIInput accountInput;
	public UIButton playButton;

	// Use this for initialization
	IEnumerator Start()
	{
		Net.Instance.Dispatcher.Register(this);
		Debug.Log(Net.Instance.Dispatcher);
		yield return null;

		Net.Instance.Open("ws://192.168.85.71:7000/shen/user");

		accountInput.value = Math.Abs(SystemInfo.deviceUniqueIdentifier.GetHashCode()).ToString();

		UIEventListener.Get(playButton.gameObject).onClick = go =>
		{
			if (string.IsNullOrEmpty(accountInput.value))
				return;
			Net.Instance.Send(new Cmd.Login.AccountTokenVerify_CS() { version = Version, gameid = GameID, account = accountInput.value, token = "dev" });
		};
	}

	void OnDestroy()
	{
		Net.Instance.Dispatcher.UnRegister(this);
		Debug.Log(Net.Instance.Dispatcher);
	}

	void Update()
	{
		playButton.isEnabled = !string.IsNullOrEmpty(accountInput.value);
	}

	[Execute]
	void Execute(AccountTokenVerify_CS cmd)
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
		Net.Instance.Open(rev.gatewayurl);
		Net.Instance.Send(new UserLoginToken_C() { logintempid = rev.logintempid, accountid = rev.accountid });
	}

	[Execute]
	void Execute(ZoneInfoList_S cmd)
	{
		ZoneListScene.ZoneInfoList = cmd;
		Application.LoadLevel("ZoneListScene");
	}
}
