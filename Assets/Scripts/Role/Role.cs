using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;

public class Role : MonoBehaviour
{
	public static Dictionary<ulong, Role> All { get; private set; }

	public MapUserData ServerInfo { get; private set; }
	private Animator animator;

	static Role()
	{
		All = new Dictionary<ulong, Role>();
	}

	private Role() { }

	public static Role Create(MapUserData info)
	{
		var item = table.TableAvatarItem.Select(info.profession, info.sexman);
		var avatar = Avatar.CreateAvatar("Prefabs/Models/Body/Sk_Female_001", item.body, item.head, item.weapon);
		avatar.name = "Role/" + info.charname;
		avatar.transform.localScale = new Vector3(5, 5, 5);
		var role = avatar.AddComponent<Role>();
		role.animator = avatar.GetComponent<Animator>();

		role.ServerInfo = info;
		return role;
	}

	[Execute]
	static void Execute(AddMapUserDataAndPosMapUserCmd_S cmd)
	{
		Role role;
		if (Role.All.TryGetValue(cmd.data.charid, out role))
		{
			role.ServerInfo = cmd.data;
		}
		else
		{
			role = Role.Create(cmd.data);
			Role.All[cmd.data.charid] = role;
		}

		Debug.Log(role.transform);
		Debug.Log(BattleScene.Instance.MapNav);
		role.transform.position = BattleScene.Instance.MapNav.GetWorldPosition(cmd.pos.x, cmd.pos.y);
	}

	[Execute]
	static void Execute(UserMoveDownMoveUserCmd_S cmd)
	{
		Role role;
		if (Role.All.TryGetValue(cmd.charid, out role))
		{
			role.transform.position = BattleScene.Instance.MapNav.GetWorldPosition(cmd.pos.x, cmd.pos.y);
		}
	}

	[Execute]
	static void Execute(UserGotoMoveUserCmd_S cmd)
	{
		Role role;
		if (Role.All.TryGetValue(cmd.charid, out role))
		{
			role.transform.position = BattleScene.Instance.MapNav.GetWorldPosition(cmd.pos.x, cmd.pos.y);
		}
	}
}
