using UnityEngine;
using System.Collections;
using GX.Net;
using System;
using Cmd;

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
			Net.Instance.Send(new AccountTokenVerifyLoginUserCmd_CS() { version = Version, gameid = GameID, account = accountInput.value, token = "dev" });
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
	void Execute(AccountTokenVerifyLoginUserCmd_CS cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
	}

	[Execute]
	void Execute(UserLoginRequestLoginUserCmd_C cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
	}

	[Execute]
	void Execute(UserLoginReturnFailLoginUserCmd_S cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
	}

	[Execute]
	void Execute(UserLoginReturnOkLoginUserCmd_S cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
		Net.Instance.Open(cmd.gatewayurl);
		Net.Instance.Send(new UserLoginTokenLoginUserCmd_C() { logintempid = cmd.logintempid, accountid = cmd.accountid });
	}

	[Execute]
	void Execute(ZoneInfoListLoginUserCmd_S cmd)
	{
		ZoneListScene.ZoneInfoList = cmd;
		Application.LoadLevel("ZoneListScene");
	}
}
