using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;

/// <summary>
/// 全局网络消息处理
/// 和具体业务相关的消息处理请直接放在其实现文件中，以保持信息的局部性
/// </summary>
public static class NetMessageProcess
{
	[Execute]
	static void Execute(MessageBoxChatUserCmd_S cmd)
	{
		MessageBox.Show(cmd.info);
	}
}
