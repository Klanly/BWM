using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System;
using Pmd;

public class LoginScene : MonoBehaviour
{
	public const int GameID = 100;
	public const string Name = "LoginScene";

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
			StopCoroutine("ConnectLoginServer");
			StartCoroutine("ConnectLoginServer", new AccountTokenVerifyLoginUserPmd_CS()
			{
				version = (uint)Pmd.Config.Version.Version_Login,
				gameid = GameID,
				account = accountInput.value,
				token = "dev",
				mid = SystemInfo.deviceUniqueIdentifier,
			});
		};
	}

	void OnDestroy()
	{
		StopCoroutine("ConnectLoginServer");
	}

	/// <summary>
	/// 连接到LoginServer，并发送指定的消息
	/// </summary>
	/// <returns></returns>
	IEnumerator ConnectLoginServer(ProtoBuf.IExtensible cmd)
	{
		var remotes = new string[]
		{
			"ws://14.17.104.56:7000/shen/user", // 广东佛山
			"ws://112.65.197.72:7000/shen/user", // 松江机房
			"ws://192.168.85.71:7000/shen/user", // 公司本地
		};

		foreach (var url in remotes)
		{
			foreach (var c in WebSocketClient.Instance.Open(url).AsEnumerable())
				yield return c;
			if (WebSocketClient.Instance.State == WebSocket.State.Open)
			{
				WebSocketClient.Instance.Send(cmd);
				yield break;
			}
			Debug.LogWarning("登陆服务器连接错误: " + url);
		}

		Debug.LogError("无法连接到登陆服务器");
		MessageBox.Show("无法连接到登陆服务器");
	}

	void Update()
	{
		//会影响按钮的按下变色，先屏蔽
		//playButton.isEnabled = !string.IsNullOrEmpty(accountInput.value);

		if (Input.GetKeyDown(KeyCode.Return))
			UIEventListener.Get(playButton.gameObject).onClick(playButton.gameObject);
	}

	/// <summary>
	/// LoginServer下发的用于网关登陆验证的令牌
	/// TODO: 加密保存或持久化
	/// </summary>
	private static UserLoginReturnOkLoginUserPmd_S gamewayToken;

	public static IEnumerator ConnectGatewayServer()
	{
		var token = gamewayToken;
		foreach (var c in WebSocketClient.Instance.Open(token.gatewayurl).AsEnumerable())
			yield return c;
		if (WebSocketClient.Instance.State == WebSocket.State.Open)
		{
			var stamp = DateTime.Now.ToUnixTime();
			WebSocketClient.Instance.Send(new UserLoginTokenLoginUserPmd_C()
			{
				gameid = token.gameid,
				zoneid = token.zoneid,
				accountid = token.accountid,
				logintempid = token.logintempid,
				timestamp = stamp,
				tokenmd5 = GX.MD5.ComputeHashString(GX.Encoding.GetBytes(
					token.accountid.ToString() +
					token.logintempid.ToString() +
					stamp.ToString() +
					token.tokenid.ToString())),
				mid = SystemInfo.deviceUniqueIdentifier,
			});
			yield break;
		}

		MessageBox.Show("无法连接到网关服务器: " + token.gatewayurl);
	}

	/// <summary>
	/// LoginServer登陆成功，连接到GatewayServer
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static IEnumerator Execute(UserLoginReturnOkLoginUserPmd_S cmd)
	{
		WebSocketClient.Instance.Close(); // 和LoginServer断开连接

		gamewayToken = cmd;
		foreach (var c in ConnectGatewayServer().AsEnumerable())
			yield return c;
	}

	[Execute]
	public static void Execute(UserLoginReturnFailLoginUserPmd_S cmd)
	{
		MessageBox.Show(cmd.retcode.ToString(), cmd.desc);
	}
}
