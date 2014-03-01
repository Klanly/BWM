using UnityEngine;
using System.Collections;
using GX.Net;
using System;
using Cmd;

public class LoginScene : MonoBehaviour
{
	public const int GameID = 100;

	public UIInput accountInput;
	public UIButton playButton;

	// Use this for initialization
	void Start()
	{
		Net.Instance.Dispatcher.Register(this);
		Debug.Log(Net.Instance.Dispatcher);

		Net.Instance.Open("ws://192.168.85.71:7000/shen/user");

		accountInput.value = Math.Abs(SystemInfo.deviceUniqueIdentifier.GetHashCode()).ToString();

		UIEventListener.Get(playButton.gameObject).onClick = go =>
		{
			if (string.IsNullOrEmpty(accountInput.value))
				return;
			Net.Instance.Send(new AccountTokenVerifyLoginUserCmd_CS() { version = (uint)Cmd.Config.Version.Version_Login, gameid = GameID, account = accountInput.value, token = "dev" });
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
	void Execute(ZoneInfoListLoginUserCmd_S cmd)
	{
		ZoneListScene.ZoneInfoList = cmd;
		Application.LoadLevel("ZoneListScene");
	}
}
