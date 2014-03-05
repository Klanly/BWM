using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System;

public class LoginScene : MonoBehaviour
{
	public const int GameID = 100;

	public UIInput accountInput;
	public UIButton playButton;

	// Use this for initialization
	void Start()
	{
		accountInput.value = System.Math.Abs(SystemInfo.deviceUniqueIdentifier.GetHashCode()).ToString();

		UIEventListener.Get(playButton.gameObject).onClick = go =>
		{
			if (string.IsNullOrEmpty(accountInput.value))
				return;
			StartCoroutine(ConnectLoginServer(new AccountTokenVerifyLoginUserCmd_CS()
			{
				version = (uint)Cmd.Config.Version.Version_Login,
				gameid = GameID,
				account = accountInput.value,
				token = "dev",
				mid = SystemInfo.deviceUniqueIdentifier,
			}));
		};
	}

	/// <summary>
	/// 连接到LoginServer
	/// </summary>
	/// <returns></returns>
	IEnumerator ConnectLoginServer(ProtoBuf.IExtensible cmd)
	{
		foreach (var c in Net.Instance.Open("ws://112.65.197.72:7000/shen/user").AsEnumerable())
			yield return c;
		if (Net.Instance.State == WebSocket.State.Open)
		{
			Net.Instance.Send(cmd);
			yield break;
		}

		foreach (var c in Net.Instance.Open("ws://192.168.85.71:7000/shen/user").AsEnumerable())
			yield return c;
		if (Net.Instance.State == WebSocket.State.Open)
		{
			Net.Instance.Send(cmd);
			yield break;
		}

		MessageBox.Show("无法连接到登陆服务器");
	}

	void Update()
	{
		//会影响按钮的按下变色，先屏蔽
		//playButton.isEnabled = !string.IsNullOrEmpty(accountInput.value);
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
	static IEnumerator Execute(UserLoginReturnOkLoginUserCmd_S cmd)
	{
		foreach (var c in Net.Instance.Open(cmd.gatewayurl).AsEnumerable())
			yield return c;
		if (Net.Instance.State == WebSocket.State.Open)
		{
			var stamp = DateTime.Now.ToUnixTime();
			Net.Instance.Send(new UserLoginTokenLoginUserCmd_C()
			{
				accountid = cmd.accountid,
				logintempid = cmd.logintempid,
				timestamp = stamp,
				tokenmd5 = GX.MD5.ComputeHashString(GX.Encoding.GetBytes(
					cmd.accountid.ToString() +
					cmd.logintempid.ToString() +
					stamp.ToString() +
					cmd.tokenid.ToString())),
				mid = SystemInfo.deviceUniqueIdentifier,
			});
			yield break;
		}

		MessageBox.Show("无法连接到网关服务器: " + cmd.gatewayurl);
	}

	[Execute]
	static void Execute(UserLoginReturnFailLoginUserCmd_S cmd)
	{
		MessageBox.Show(cmd.retcode.ToString(), cmd.desc);
	}
}
