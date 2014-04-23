using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;
using System;
using GX;

public class Role : MonoBehaviour
{
	public static ObservableDictionary<ulong, Role> All { get; private set; }

	public MapUserData ServerInfo { get; private set; }

	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	private Entity entity;
	//private Animator animator;
	private Move move;

	static Role()
	{
		All = new ObservableDictionary<ulong, Role>();
	}

	private Role() { }

	public static Role Create(MapUserData info)
	{
		var tbl = table.TableAvatar.Where(info.profession, info.sexman);
		var avatar = Avatar.Create(tbl);
#if UNITY_EDITOR
		avatar.name = "Role." + info.charname;
#endif
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
#if UNITY_EDITOR
		headTip.name = role.name;
#endif
		headTip.text = role.ServerInfo.charname;
		headTip.hideIfOffScreen = true;
		headTip.SetAnchor(role.gameObject);
		headTip.bottomAnchor.absolute = 120;
		headTip.topAnchor.absolute = headTip.bottomAnchor.absolute + 30;
	}

	private CastSkill m_caseSkillCache;
	public CastSkill CastSkill
	{
		get
		{
			if (m_caseSkillCache == null)
				this.m_caseSkillCache = this.gameObject.AddComponent<CastSkill>();
			return m_caseSkillCache;
		}
	}

	#region 网络消息 角色移动
	[Execute]
	public static void Execute(AddMapUserDataAndPosMapUserCmd_S cmd)
	{
		if (MainRole.ServerInfo == null)
			return;
		var role = Role.All[cmd.data.charid];
		if (role != null)
		{
			role.ServerInfo = cmd.data;
		}
		else
		{
			if (cmd.data.charid == MainRole.ServerInfo.userdata.charid)
			{
				var mainRole = MainRole.Create();
				role = mainRole.GetComponent<Role>();
			}
			else
			{
				role = Role.Create(cmd.data);
			}
			
			Role.All[cmd.data.charid] = role;
		}

		role.entity.Grid = cmd.pos;
	}

	[Execute]
	public static void Execute(RemoveMapUserMapUserCmd_S cmd)
	{
		var role = Role.All[cmd.charid];
		if (role != null)
		{
			Role.All.Remove(cmd.charid);
			GameObject.Destroy(role.gameObject);
		}
		if (SelectTarget.Selected != null && SelectTarget.Selected.entrytype == SceneEntryType.SceneEntryType_Player && SelectTarget.Selected.entryid == cmd.charid)
			SelectTarget.Selected = null;
	}

	[Execute]
	public static void Execute(UserMoveDownMoveUserCmd_S cmd)
	{
		if (MainRole.ServerInfo == null || cmd.charid == MainRole.ServerInfo.userdata.charid)
			return;

		var role = Role.All[cmd.charid];
		if (role != null)
		{
			role.move.TargetPosition = BattleScene.Instance.MapNav.GetWorldPosition(cmd.poscm);
		}
	}

	[Execute]
	public static void Execute(UserGotoMoveUserCmd_S cmd)
	{
		var role = Role.All[cmd.charid];
		if (role != null)
		{
			role.entity.Position = BattleScene.Instance.MapNav.GetWorldPosition(cmd.poscm);
		}
	}
	#endregion

	#region 网络消息 角色血量变化
	[Execute]
	public static void Execute(SetUserHpSpDataUserCmd_S cmd)
	{
		var my = MainRole.Instance;
		if (my != null && cmd.charid == my.Role.ServerInfo.charid)
		{
			my.maxhp = cmd.maxhp;
			MainRole.ServerInfo.hp = cmd.hp;
			my.maxsp = cmd.maxsp;
			MainRole.ServerInfo.sp = cmd.sp;
			return;
		}

		if (SelectTarget.Selected != null && 
			SelectTarget.Selected.entrytype == SceneEntryType.SceneEntryType_Player && 
			SelectTarget.Selected.entryid == cmd.charid)
		{
			var view = BattleScene.Instance.Gui<SelectTargetRole>();
			view.SetHp(cmd.hp, cmd.maxhp);
			view.SetSp(cmd.sp, cmd.maxsp);
			return;
		}
	}

	[Execute]
	public static void Execute(ChangeUserHpDataUserCmd_S cmd)
	{
		if (MainRole.ServerInfo != null && cmd.charid == MainRole.ServerInfo.userdata.charid)
		{
			MainRole.ServerInfo.hp = cmd.curhp;
			return;
		}
		if (SelectTarget.Selected != null &&
			SelectTarget.Selected.entrytype == SceneEntryType.SceneEntryType_Player &&
			SelectTarget.Selected.entryid == cmd.charid)
		{
			var view = BattleScene.Instance.Gui<SelectTargetRole>();
			view.SetHp(cmd.curhp);
			return;
		}
	}

	[Execute]
	public static void Execute(ChangeUserSpDataUserCmd_S cmd)
	{
		if (MainRole.ServerInfo != null && cmd.charid == MainRole.ServerInfo.userdata.charid)
		{
			MainRole.ServerInfo.sp = cmd.cursp;
			return;
		}
		if (SelectTarget.Selected.entrytype == SceneEntryType.SceneEntryType_Player && SelectTarget.Selected.entryid == cmd.charid)
		{
			var view = BattleScene.Instance.Gui<SelectTargetRole>();
			view.SetSp(cmd.cursp);
			return;
		}
	}

	/// <summary>
	/// 角色升级
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static void Execute(UserLevelUpMapUserCmd_S cmd)
	{
		MainRole.Execute(cmd);
		var role = All[cmd.charid];
		if(role == null)
			return;
		// TODO: 播放升级特效
	}
	#endregion
}
