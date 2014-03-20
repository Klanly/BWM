using UnityEngine;
using System.Collections;
using Cmd;
using System.Collections.Generic;
using System.Linq;
using GX.Net;

public class ItemManager : IEnumerable<SaveItem>
{
	public static ItemManager Instance { get; set; }
	private readonly List<SaveItem> items = new List<SaveItem>();

	public SaveItem this[ulong thisid] { get { return items.Find(i => i.thisid == thisid); } }
	public SaveItem this[ItemLocation loc] { get { return this[loc.type, loc.index]; } }
	public SaveItem this[ItemLocation.PackageType type, int index] { get { return items.Find(i => i.loc.type == type && i.loc.index == index); } }

	public void Clear()
	{
		items.Clear();
	}

	protected bool Remove(ulong thisid)
	{
		var index = items.FindIndex(i => i.thisid == thisid);
		if (index >= 0)
		{
			items.RemoveAt(index);
			return true;
		}
		return false;
	}

	protected bool Swap(ulong srcThisid, ulong dstThisid)
	{
		var src = this[srcThisid];
		var dst = this[dstThisid];
		if (src != null && dst != null)
		{
			var temp = src.loc;
			src.loc = dst.loc;
			dst.loc = temp;
			return true;
		}
		return false;
	}

	protected bool Splite(ulong thisid, int takeNum, ItemLocation dst)
	{
		var item = this[thisid];
		if (item != null && item.num > takeNum && this[dst] == null)
		{
			item.num -= takeNum;
			var copy = item.DeepClone();
			copy.num = takeNum;
			copy.loc = dst;
			this.items.Add(copy);
			return true;
		}
		return false;
	}

	protected bool Union(ulong srcThisid, ulong dstThisid)
	{
		var src = items.FindIndex(i => i.thisid == srcThisid);
		var dst = items.FindIndex(i => i.thisid == dstThisid);
		if (src >= 0 && dst >= 0 && items[src].baseid == items[dst].baseid)
		{
			items[dst].num += items[src].num;
			items.RemoveAt(src);
			return true;
		}
		return false;
	}

	#region IEnumerable<SaveItem> 成员

	public IEnumerator<SaveItem> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	#endregion

	#region IEnumerable 成员

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	#endregion

	#region 网络消息处理
	[Execute]
	static void Execute(AddItemListItemUserCmd_S cmd)
	{
		ItemManager.Instance.items.AddRange(cmd.itemlist);
	}

	[Execute]
	static void Execute(RemoveItemItemUserCmd_CS cmd)
	{
		ItemManager.Instance.Remove(cmd.thisid);
	}

	[Execute]
	static void Execute(SwapItemItemUserCmd_CS cmd)
	{
		ItemManager.Instance.Swap(cmd.srcThisid, cmd.dstThisid);
	}

	[Execute]
	static void Execute(SplitItemItemUserCmd_CS cmd)
	{
		ItemManager.Instance.Splite(cmd.thisid, cmd.num, cmd.dst);
	}

	[Execute]
	static void Execute(UnionItemItemUserCmd_CS cmd)
	{
		ItemManager.Instance.Union(cmd.srcThisid, cmd.dstThisid);
	}

	[Execute]
	static void Execute(RefreshPosItemUserCmd_CS cmd)
	{
		ItemManager.Instance[cmd.thisid].loc = cmd.dst;
	}

	[Execute]
	static void Execute(RefreshCountItemItemUserCmd_CS cmd)
	{
		ItemManager.Instance[cmd.thisid].num = cmd.count;
	}
	#endregion
}
