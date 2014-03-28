// 表格相关的扩充代码
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cmd;

namespace table
{
	#region TableMap
	partial class TableMap
	{
		public static TableMap Where(uint id)
		{
			return (from i in Table.Query<TableMap>() where i.id == id select i).First();
		}
	}
	#endregion

	#region TableAvatar
	partial class TableAvatar
	{
		public bool sexman { get { return this.sex == 1; } }

		public static TableAvatar Where(Profession profession, bool sexman)
		{
			var pro = (uint)profession;
			var sex = sexman ? 1 : 2;
			return (from i in Table.Query<TableAvatar>() where i.profession == pro && i.sex == sex select i).First();
		}
	}
	#endregion

	#region TableItem
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
	#endregion

	#region TableItemType
	partial class TableItemType
	{
		public bool IsEquip { get { return this.equipPos != 0; } }

		public EquipPos EquipPos { get { return (EquipPos)this.equipPos; } }
	}
	#endregion

	#region TableNpc
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
	#endregion

	#region TableSkill
	partial class TableSkill
	{
		public Profession Profession { get { return (Profession)this.profession; } }

		public static TableSkill Where(SaveSkill ss)
		{
			return Table.Query<TableSkill>().First(i => i.id == ss.skillid && i.level == ss.level);
		}

		public override string ToString()
		{
			return string.Format("{0},{1} {2}", this.id, this.level, this.name);
		}
	}
	#endregion
}

public static class TableExtensions
{
}
