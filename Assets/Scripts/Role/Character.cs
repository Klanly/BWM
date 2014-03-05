using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;
using CharacterInfo = Cmd.AddMapUserDataAndPosMapUserCmd_S;

public class Character : MonoBehaviour
{
	public static Dictionary<ulong, CharacterInfo> All { get; private set; }

	static Character()
	{
		All = new Dictionary<ulong, CharacterInfo>();
	}

	[Execute]
	static void Execute(CharacterInfo cmd)
	{
		if (Character.All.ContainsKey(cmd.data.charid) == false)
		{
			// TODO: add charater to scene
		}
		Character.All[cmd.data.charid] = cmd;
	}

	[Execute]
	static void Execute(UserMoveDownMoveUserCmd_S cmd)
	{
		CharacterInfo info;
		if (Character.All.TryGetValue(cmd.charid, out info))
			info.pos = cmd.pos;
	}

	[Execute]
	static void Execute(UserGotoMoveUserCmd_S cmd)
	{
		CharacterInfo info;
		if (Character.All.TryGetValue(cmd.charid, out info))
		{
			info.pos = cmd.pos;
			// TODO: process cmd.mapid
		}
	}
}
