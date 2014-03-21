using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Entity))]
public class Npc : MonoBehaviour
{
#if UNITY_EDITOR
	/// <summary>
	/// NPC表中的ID
	/// </summary>
	public int baseId;
	/// <summary>
	/// 别名
	/// </summary>
	public string alias;
	/// <summary>
	/// 重生时间(秒)
	/// </summary>
	public int relivetime;
	/// <summary>
	/// 刷新概率
	/// </summary>
	public int rate = 100;
#endif

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
	static void Execute(AddMapNpcDataAndPosMapUserCmd_S cmd)
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
}
