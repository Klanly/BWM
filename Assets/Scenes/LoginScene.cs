using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;

public class LoginScene : MonoBehaviour
{
	public const int GameID = 100;

	public UIInput accountInput;
	public UIButton playButton;

	// Use this for initialization
	void Start()
	{
		//Net.Instance.Open("ws://192.168.85.71:7000/shen/user");
		Net.Instance.Open("ws://112.65.197.72:7000/shen/user");

		accountInput.value = System.Math.Abs(SystemInfo.deviceUniqueIdentifier.GetHashCode()).ToString();

		UIEventListener.Get(playButton.gameObject).onClick = go =>
		{
			if (string.IsNullOrEmpty(accountInput.value))
				return;
			// 连接到LoginServer
			Net.Instance.Send(new AccountTokenVerifyLoginUserCmd_CS() { version = (uint)Cmd.Config.Version.Version_Login, gameid = GameID, 
				account = accountInput.value,
				token = "dev" });
		};
	}

	void Update()
	{
		playButton.isEnabled = !string.IsNullOrEmpty(accountInput.value);
	}

	[Execute]
	static void Execute(AccountTokenVerifyLoginUserCmd_CS cmd)
	{
	}

	[Execute]
	static void Execute(UserLoginRequestLoginUserCmd_C cmd)
	{
	}

	/// <summary>
	/// LoginServer登陆成功，连接到GatewayServer
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	static void Execute(UserLoginReturnOkLoginUserCmd_S cmd)
	{
		Net.Instance.Open(cmd.gatewayurl);
		Net.Instance.Send(new UserLoginTokenLoginUserCmd_C() { logintempid = cmd.logintempid, accountid = cmd.accountid });
	}

	[Execute]
	static void Execute(UserLoginReturnFailLoginUserCmd_S cmd)
	{
		MessageBox.Show(cmd.retcode.ToString(), cmd.desc);
	}
}
