using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;
using System;

public class Role : MonoBehaviour
{
	public static Dictionary<ulong, Role> All { get; private set; }

	public MapUserData ServerInfo { get; private set; }

	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	private Entity entity;
	//private Animator animator;
	private Move move;

	static Role()
	{
		All = new Dictionary<ulong, Role>();
	}

	private Role() { }

	public static Role Create(MapUserData info)
	{
		var tbl = table.TableAvatar.Select(info.profession, info.sexman);
		var avatar = Avatar.Create(tbl);
		avatar.name = "Role." + info.charname;
		avatar.transform.localScale = new Vector3(5, 5, 5);

		var role = avatar.AddComponent<Role>();
		role.entity = avatar.AddComponent<Entity>();
		role.move = avatar.AddComponent<Move>();
		//role.animator = avatar.GetComponent<Animator>();
		role.ServerInfo = info;

		CreateHeadTip(role);
		
		return role;
	}

	/// <summary>
	/// 角色头顶文字
	/// </summary>
	/// <param name="role"></param>
	private static void CreateHeadTip(Role role)
	{
		var headTip = (GameObject.Instantiate(Resources.Load("Prefabs/Gui/HeadTip")) as GameObject).GetComponent<UILabel>();
		headTip.name = role.name;
		headTip.text = role.ServerInfo.charname;
		headTip.hideIfOffScreen = true;
		headTip.SetAnchor(role.gameObject);
		headTip.bottomAnchor.absolute = 120;
		headTip.topAnchor.absolute = headTip.bottomAnchor.absolute + 30;
	}

	[Execute]
	public static void Execute(AddMapUserDataAndPosMapUserCmd_S cmd)
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

		role.entity.Grid = cmd.pos;
	}

	[Execute]
	public static void Execute(UserMoveDownMoveUserCmd_S cmd)
	{
		if (cmd.charid == MainRole.ServerInfo.charid)
			return;

		Role role;
		if (Role.All.TryGetValue(cmd.charid, out role))
		{
			role.move.TargetPosition = BattleScene.Instance.MapNav.GetWorldPosition(cmd.pos);
		}
	}

	[Execute]
	public static void Execute(UserGotoMoveUserCmd_S cmd)
	{
		Role role;
		if (Role.All.TryGetValue(cmd.charid, out role))
		{
			role.entity.Position = BattleScene.Instance.MapNav.GetWorldPosition(cmd.pos);
		}
	}
}
