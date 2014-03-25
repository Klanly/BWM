using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System;

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
	static void Execute(MessageBoxChatUserCmd_S cmd)
	{
		MessageBox.Show(cmd.info);
	}

	/// <summary>
	/// 服务器对客户端的时间探测，可用于反外挂等时间相关业务
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	static void Execute(TickRequestNullUserCmd_CS cmd)
	{
		Net.Instance.Send(new TickReturnNullUserCmd_CS() { requesttime = cmd.requesttime, mytime = DateTime.Now.ToUnixTime() });
	}

	/// <summary>
	/// 场景点选
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	static void Execute(SelectSceneEntryScriptUserCmd_CS cmd)
	{
		switch (cmd.entrytype)
		{
			case SceneEntryType.SceneEntryType_Npc:
				Npc.SceneSelect(cmd.entryid);
				break;
			case SceneEntryType.SceneEntryType_Player:
				Role.SceneSelect(cmd.entryid);
				break;
			default:
				break;
		}
	}
}
