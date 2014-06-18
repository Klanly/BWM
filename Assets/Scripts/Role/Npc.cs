using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using System.Collections.Generic;
using System.Linq;
using GX;

public class Npc : MonoBehaviour
{
	public static ObservableDictionary<ulong, Npc> All { get; private set; }
	public MapNpcData ServerInfo { get; private set; }
	public table.TableNpc TableInfo { get; private set; }

	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	private Entity entity;
	private Animator animator;
	private Move move;

	static Npc()
	{
		All = new ObservableDictionary<ulong, Npc>();
	}

	private Npc() { }

	private CastSkill m_castSkillCache;
	public CastSkill CastSkill
	{
		get
		{
			if (m_castSkillCache == null)
				this.m_castSkillCache = this.gameObject.AddComponent<CastSkill>();
			return m_castSkillCache;
		}
	}

	public static Npc Create(uint baseid, MapNpcData info)
	{
		var tbl = Table.Query<table.TableNpc>().First(i => i.id == baseid);

		var res = Resources.Load(tbl.model);
		if (res == null)
		{
			Debug.LogError("Load Resources error: " + tbl.model);
			return null;
		}

		var avatar = Object.Instantiate(res) as GameObject;
#if UNITY_EDITOR
		avatar.name = string.Format("Npc.{0}({1})", tbl.name, info.tempid);
#endif
		avatar.transform.localScale = new Vector3(5, 5, 5);

		var npc = avatar.AddComponent<Npc>();
		npc.entity = avatar.AddComponent<Entity>();
		npc.move = avatar.AddComponent<Move>();
		npc.move.speed = sender => npc.ServerInfo.movespeed * 0.01f;
		npc.animator = avatar.GetComponent<Animator>();
		npc.ServerInfo = info;
		npc.TableInfo = tbl;

		CreateHeadTip(npc);

		return npc;
	}

	/// <summary>
	/// NPC头顶文字
	/// </summary>
	/// <param name="npc"></param>
	private static void CreateHeadTip1(Npc npc)
	{
		var headTip = (GameObject.Instantiate(Resources.Load("Prefabs/Gui/HeadTip")) as GameObject).GetComponent<UILabel>();
#if UNITY_EDITOR
		headTip.name = npc.name;
#endif
		headTip.text = npc.TableInfo.name;
		headTip.hideIfOffScreen = true;
		headTip.SetAnchor(npc.gameObject);
		headTip.bottomAnchor.absolute = 120;
		headTip.topAnchor.absolute = headTip.bottomAnchor.absolute + 30;

		var recycle = npc.gameObject.AddComponent<OnDestroyAction>();
		recycle.Action = () => { try { NGUITools.Destroy(headTip.gameObject); } catch { } };
	}
	private static void CreateHeadTip(Npc npc)
	{
		var go = GameObject.Instantiate(Resources.Load("Prefabs/Gui/HeadTipNpc")) as GameObject;
#if UNITY_EDITOR
		go.name = npc.name;
#endif
		var headTip = go.GetComponent<HeadTip>();
		headTip.text.text = npc.TableInfo.name;
		headTip.hpText.text = "100/100";
		headTip.hpProgress.value = 0.8f;

		var widget = go.GetComponent<UIWidget>();
		widget.hideIfOffScreen = true;
		widget.SetAnchor(npc.gameObject);
		widget.bottomAnchor.absolute = 120;
		widget.topAnchor.absolute = widget.bottomAnchor.absolute + 30;

		var recycle = npc.gameObject.AddComponent<OnDestroyAction>();
		recycle.Action = () => { try { NGUITools.Destroy(headTip.gameObject); } catch { } };
	}

	#region 网络消息 NPC移动
	[Execute]
	public static void Execute(AddMapNpcDataAndPosMapUserCmd_S cmd)
	{
		var npc = Npc.All[cmd.data.tempid];
		if (npc != null)
		{
			npc.ServerInfo = cmd.data;
		}
		else
		{
			npc = Npc.Create(cmd.baseid, cmd.data);
			Npc.All[cmd.data.tempid] = npc;
		}

		npc.entity.Grid = new MapGrid(cmd.poscm);
	}

	[Execute]
	public static void Execute(RemoveMapNpcMapUserCmd_S cmd)
	{
		var npc = Npc.All[cmd.tempid];
		if (npc != null)
		{
			Npc.All.Remove(cmd.tempid);
			GameObject.Destroy(npc.gameObject);
		}
	}

	[Execute]
	public static void Execute(NpcMoveDownMoveUserCmd_S cmd)
	{
		var target = Npc.All[cmd.tempid];
		if (target != null)
		{
			target.move.TargetPosition = BattleScene.Instance.MapNav.GetWorldPosition(new MapGrid(cmd.poscm));
		}
	}
	#endregion

	#region 网络消息 NPC血量变化
	[Execute]
	public static void Execute(SetNpcHpDataUserCmd_S cmd)
	{
		var target = All[cmd.tempid];
		if (target == null)
			return;
		target.ServerInfo.maxhp = cmd.maxhp;
		target.ServerInfo.hp = cmd.hp;
		SelectTarget.OnUpdate(target);
		//如果死亡,死亡动画
		target.animator.SetInteger("hp", target.ServerInfo.hp); ;
	}

	[Execute]
	public static void Execute(ChangeNpcHpDataUserCmd_S cmd)
	{
		var target = All[cmd.tempid];
		if (target == null)
			return;
		target.ServerInfo.hp = cmd.curhp;
		SelectTarget.OnUpdate(target);
		//如果死亡,死亡动画
		target.animator.SetInteger("hp", target.ServerInfo.hp); ;
	}
	#endregion
}
