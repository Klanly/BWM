using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using System.Collections.Generic;
using System.Linq;

public class Npc : MonoBehaviour
{
	public static Dictionary<ulong, Npc> All { get; private set; }
	public MapNpcData ServerInfo { get; private set; }
	public table.TableNpc TableInfo { get; private set; }

	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	private Entity entity;
	//private Animator animator;
	//private Move move;

	static Npc()
	{
		All = new Dictionary<ulong, Npc>();
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
		avatar.name = string.Format("Npc.{0}({1})", tbl.name, info.tempid);
		avatar.transform.localScale = new Vector3(5, 5, 5);

		var npc = avatar.AddComponent<Npc>();
		npc.entity = avatar.AddComponent<Entity>();
		//npc.move = avatar.AddComponent<Move>();
		//npc.animator = avatar.GetComponent<Animator>();
		npc.ServerInfo = info;
		npc.TableInfo = tbl;

		CreateHeadTip(npc);

		return npc;
	}

	/// <summary>
	/// NPC头顶文字
	/// </summary>
	/// <param name="npc"></param>
	private static void CreateHeadTip(Npc npc)
	{
		var headTip = (GameObject.Instantiate(Resources.Load("Prefabs/Gui/HeadTip")) as GameObject).GetComponent<UILabel>();
		headTip.name = npc.name;
		headTip.text = npc.TableInfo.name;
		headTip.hideIfOffScreen = true;
		headTip.SetAnchor(npc.gameObject);
		headTip.bottomAnchor.absolute = 120;
		headTip.topAnchor.absolute = headTip.bottomAnchor.absolute + 30;
	}

	[Execute]
	public static void Execute(AddMapNpcDataAndPosMapUserCmd_S cmd)
	{
		Npc npc;
		if (Npc.All.TryGetValue(cmd.data.tempid, out npc))
		{
			npc.ServerInfo = cmd.data;
		}
		else
		{
			npc = Npc.Create(cmd.baseid, cmd.data);
			Npc.All[cmd.data.tempid] = npc;
		}

		npc.entity.Grid = cmd.pos;
	}

	[Execute]
	public static void Execute(ChangeNpcHpDataUserCmd_S cmd)
	{
		// TODO: NPC血量更新未处理
	}

	[Execute]
	public static void Execute(SetNpcHpDataUserCmd_S cmd)
	{
		// TODO: NPC血量更新未处理
	}
}
