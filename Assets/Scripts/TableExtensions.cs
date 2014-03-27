// 表格相关的扩充代码
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cmd;

namespace table
{
	partial class TableMap
	{
		public static TableMap Select(uint id)
		{
			return (from i in Table.Query<TableMap>() where i.id == id select i).First();
		}
	}

	partial class TableAvatar
	{
		public bool sexman { get { return this.sex == 1; } }

		public static TableAvatar Select(Profession profession, bool sexman)
		{
			var pro = (uint)profession;
			var sex = sexman ? 1 : 2;
			return (from i in Table.Query<TableAvatar>() where i.profession == pro && i.sex == sex select i).First();
		}
	}

	partial class TableItem
	{
		public Profession Profession { get { return (Profession)this.profession; } }

		private table.TableItemType itemTypeCache;
		public table.TableItemType Type
		{
			get
			{
				if (itemTypeCache == null || itemTypeCache.type != this.type)
					itemTypeCache = Table.Query<table.TableItemType>().First(i => i.type == this.type);
				return itemTypeCache;
			}
		}
	}

	partial class TableItemType
	{
		public bool IsEquip { get { return this.equipPos != 0; } }

		public EquipPos EquipPos { get { return (EquipPos)this.equipPos; } }
	}

	public enum NpcType
	{
		Npc = 0,
		/// <summary>
		/// 普通怪物
		/// </summary>
		Monster = 1,
		/// <summary>
		/// 精英怪物
		/// </summary>
		Elite = 2,
		Boss = 3,
		/// <summary>
		/// 宝箱
		/// </summary>
		Box = 4,
	}
	partial class TableNpc
	{
		public NpcType Type { get { return (NpcType)this.type; } }
	}
}

public static class TableExtensions
{
}
