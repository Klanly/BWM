using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;
using RoleInfo = Cmd.AddMapUserDataAndPosMapUserCmd_S;

public class Role : MonoBehaviour
{
	public static Dictionary<ulong, RoleInfo> All { get; private set; }

	static Role()
	{
		All = new Dictionary<ulong, RoleInfo>();
	}

	[Execute]
	static void Execute(RoleInfo cmd)
	{
		if (Role.All.ContainsKey(cmd.data.charid) == false)
		{
			// TODO: add charater to scene
		}
		Role.All[cmd.data.charid] = cmd;
	}

	[Execute]
	static void Execute(UserMoveDownMoveUserCmd_S cmd)
	{
		RoleInfo info;
		if (Role.All.TryGetValue(cmd.charid, out info))
			info.pos = cmd.pos;
	}

	[Execute]
	static void Execute(UserGotoMoveUserCmd_S cmd)
	{
		RoleInfo info;
		if (Role.All.TryGetValue(cmd.charid, out info))
		{
			info.pos = cmd.pos;
			// TODO: process cmd.mapid
		}
	}
}
