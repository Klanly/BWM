using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using System.Collections.Generic;
using System.Linq;
using GX;

public class SceneItem : MonoBehaviour
{
	public static ObservableDictionary<ulong, SceneItem> All { get; private set; }

	public SaveItem ServerInfo { get; private set; }


	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	private Entity entity;
	private Animator animator;

	static SceneItem()
	{
		All = new ObservableDictionary<ulong, SceneItem>();
	}

	private SceneItem() { }


	public static SceneItem Create(SaveItem info)
	{
		var tbl = Table.Query<table.TableItem>().First(i => i.id == info.baseid);

		var res = Resources.Load(tbl.model);
		if (res == null)
		{
			Debug.LogError("Load Resources error: " + tbl.model);
			return null;
		}

		var avatar = Object.Instantiate(res) as GameObject;
#if UNITY_EDITOR
		avatar.name = string.Format("SceneItem.{0}({1})", tbl.name, info.thisid);
#endif
		avatar.transform.localScale = new Vector3(5, 5, 5);

		var item = avatar.AddComponent<SceneItem>();
		item.entity = avatar.AddComponent<Entity>();
		item.animator = avatar.GetComponent<Animator>();
		item.ServerInfo = info;

		return item;
	}



	#region 网络消息 添加场景道具
	[Execute]
	public static void Execute(AddMapItemMapUserCmd_S cmd)
	{
		var item = SceneItem.All[cmd.item.thisid];
		if (item != null)
		{
			item.ServerInfo = cmd.item;
		}
		else
		{
			item = SceneItem.Create(cmd.item);
			SceneItem.All[cmd.item.thisid] = item;
		}
		var pos = cmd.item.loc.pos;
		pos.x = pos.x * 25;
		pos.y = pos.y * 25;
		item.entity.Grid = new MapGrid(cmd.item.loc.pos);
	}

	[Execute]
	public static void Execute(RemoveMapItemMapUserCmd_S cmd)
	{
		var item = SceneItem.All[cmd.thisid];
		if (item != null)
		{
			SceneItem.All.Remove(cmd.thisid);
			GameObject.Destroy(item.gameObject);
		}
	}
	#endregion
}
