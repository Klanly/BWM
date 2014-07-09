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

	private MapUserData m_serverInfo;
	public MapUserData ServerInfo 
	{
		get 
		{
			if (this.m_serverInfo != null && MainRole.ServerInfo != null && this.m_serverInfo.charid == MainRole.ServerInfo.userdata.charid)
				return MainRole.ServerInfo.userdata;
			return m_serverInfo; 
		}
		private set
		{
			if (value == m_serverInfo)
				return;
			m_serverInfo = value;

			if (this.m_serverInfo != null && MainRole.ServerInfo != null && this.m_serverInfo.charid == MainRole.ServerInfo.userdata.charid)
			{
				//Assert(this == MainRole.Role);
				MainRole.ServerInfo.userdata = this.m_serverInfo;
				if (MainRole.Instance != null)
					MainRole.Instance.OnPropertyChanged("userdata");
			}
		}
	}

	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	public Entity entity;
	public Animator animator;
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
        avatar.AddComponent<HpRole>();
		role.move.speed = sender => role.ServerInfo.movespeed * 0.01f;
		role.animator = avatar.GetComponent<Animator>();
		role.animator.SetInteger("profession", (int)info.profession);
		role.ServerInfo = info;
        avatar.AddComponent<Entry>();

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

		var recycle = role.gameObject.AddComponent<OnDestroyAction>();
		recycle.Action = () => { try { NGUITools.Destroy(headTip.gameObject); } catch { } };
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

		role.entity.Grid = new MapGrid(cmd.poscm);
	}

	[Execute]
	public static void Execute(RemoveMapUserMapUserCmd_S cmd)
	{
		var role = Role.All[cmd.charid];
		if (role != null)
		{
			if (MainRole.Instance != null && cmd.charid == MainRole.Instance.Role.ServerInfo.charid)
			{
				Debug.Log("忽略主角删除消息");
				return;
			}
			Role.All.Remove(cmd.charid);
			GameObject.Destroy(role.gameObject);
		}
	}

	[Execute]
	public static void Execute(UserMoveDownMoveUserCmd_S cmd)
	{
		if (MainRole.ServerInfo == null || cmd.charid == MainRole.ServerInfo.userdata.charid)
			return;

		var role = Role.All[cmd.charid];
		if (role != null)
		{
			role.move.TargetPosition = BattleScene.Instance.MapNav.GetWorldPosition(new MapGrid(cmd.poscm));
		}
	}

	[Execute]
	public static void Execute(UserGotoMoveUserCmd_S cmd)
	{
		var role = Role.All[cmd.charid];
		if (role != null)
		{
			role.entity.Position = BattleScene.Instance.MapNav.GetWorldPosition(new MapGrid(cmd.poscm));
		}
	}
	#endregion

	#region 网络消息 角色血量变化
	[Execute]
	public static void Execute(SetUserHpSpDataUserCmd_S cmd)
	{
		if (MainRole.Execute(cmd))
			return;

		var target = All[cmd.charid];
		if (target == null) 
			return;

		target.gameObject.GetComponent<HpProtocol>().hp = cmd.hp;
        target.gameObject.GetComponent<HpProtocol>().maxhp = cmd.maxhp;
		//target.SetSp(cmd.sp, cmd.maxsp);
	}

	[Execute]
	public static void Execute(ChangeUserHpDataUserCmd_S cmd)
	{
		// 技能和buff影响的血量由技能系统修改
		if (cmd.changetype > 0)
			return;

		var target = All[cmd.charid];
		if (target == null) 
			return;

        target.gameObject.GetComponent<HpProtocol>().hp = cmd.curhp;
	}

	[Execute]
	public static void Execute(ChangeUserSpDataUserCmd_S cmd)
	{
		if (MainRole.Execute(cmd))
			return;

		var target = All[cmd.charid];
		if (target == null) 
			return;
		
		//target.SetSp(cmd.cursp);
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

		// 角色升级特效
		GameApplication.PlayEffect("Prefabs/Models/Effect/shengji_sz", role.transform);
	}
	#endregion
}
