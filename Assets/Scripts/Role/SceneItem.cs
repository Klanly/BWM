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
	public table.TableItem TableInfo { get; private set; }

	private Entity entity;
	private Animator animator;

	/// <summary>
	/// 上次检测拾取的时间，免得多次发送消息
	/// </summary>
	private float lastPickUpTime = 0;
	/// <summary>
	/// 检测拾取的冷却
	/// </summary>
	private float deltaPickUpTime = 1.0f;

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
		//item.animator = avatar.GetComponent<Animator>();
		item.ServerInfo = info;
		item.TableInfo = tbl;
		CreateHeadTip(item);

		item.lastPickUpTime = Time.time;
		if (MainRole.Instance != null)
			MainRole.Instance.entity.PositionChanged += item.OnMainRolePositionChanged;
		item.StartCoroutine("WaitAndPickUp", item.deltaPickUpTime);
		return item;
	}

	void OnDestroy()
	{
		StopCoroutine("WaitAndPickUp");
		if (MainRole.Instance != null)
			MainRole.Instance.entity.PositionChanged -= OnMainRolePositionChanged;
	}

	/// <summary>
	/// 道具头顶文字
	/// </summary>
	/// <param name="npc"></param>
	private static void CreateHeadTip(SceneItem item)
	{
		var headTip = (GameObject.Instantiate(Resources.Load("Prefabs/Gui/HeadTipItem")) as GameObject).GetComponent<UILabel>();
#if UNITY_EDITOR
		headTip.name = item.name;
#endif
		headTip.text = item.TableInfo.name;
		headTip.hideIfOffScreen = true;
		headTip.SetAnchor(item.gameObject);
		headTip.bottomAnchor.absolute = 120;
		headTip.topAnchor.absolute = headTip.bottomAnchor.absolute + 30;

		var recycle = item.gameObject.AddComponent<OnDestroyAction>();
		recycle.Action = () => { try { NGUITools.Destroy(headTip.gameObject); } catch { } };
	}

	/// <summary>
	/// 主角位置改变时，检查是否可拾取
	/// </summary>
	/// <param name="sender">Sender.</param>
	void OnMainRolePositionChanged(Entity sender)
	{
		PickUp();
	}

	IEnumerator WaitAndPickUp(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		PickUp();
	}

	public bool PickUp()
	{
		if ((Time.time - lastPickUpTime) > deltaPickUpTime)
		{
			if (MainRole.Instance != null && gameObject.GetComponent<Entity>() != null)
			{
				if(Vector3.Distance(MainRole.Instance.entity.Position,entity.Position) < 1.5f)
				{
					lastPickUpTime = Time.time;
					Net.Instance.Send(new Cmd.PickUpItemPropertyUserCmd_C()
					{
						thisid = this.ServerInfo.thisid
					});
					return true;
				}
			}
		}
		return false;
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
	public static void Execute(ReplaceItemListMapUserCmd_S cmd)
	{
		SceneItem.All.Clear();
		foreach(var v in cmd.itemlist)
		{
			var item = SceneItem.Create(v);
			SceneItem.All[v.thisid] = item;
			var pos = v.loc.pos;
			pos.x = pos.x * 25;
			pos.y = pos.y * 25;
			item.entity.Grid = new MapGrid(v.loc.pos);
		}

		
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
