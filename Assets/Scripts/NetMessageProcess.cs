using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System;
using Pmd;

/// <summary>
/// 全局网络消息处理
/// 和具体业务相关的消息处理请直接放在其实现文件中，以保持信息的局部性
/// </summary>
public static class NetMessageProcess
{
	/// <summary>
	/// 全局MessageBox
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static void Execute(MessageBoxLoginUserPmd_S cmd)
	{
		MessageBox.Show(cmd.info);
	}

	/// <summary>
	/// 服务器对客户端的时间探测，可用于反外挂等时间相关业务
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static void Execute(TickRequestNullUserPmd_CS cmd)
	{
		WebSocketClient.Instance.Send(new TickReturnNullUserPmd_CS() { requesttime = cmd.requesttime, mytime = DateTime.Now.ToUnixTime() });
	}
}
