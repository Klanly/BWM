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
	partial class TableNpc
	{
		public NpcBaseType BaseType { get { return (NpcBaseType)this.baseType; } }
		public NpcClickType ClickType { get { return (NpcClickType)this.clickType; } }
	}
	#endregion

	#region TableSkill
	partial class TableSkill
	{
		public Profession Profession { get { return (Profession)this.profession; } }

		/// <summary>
		/// 相同<see cref="id"/>的最高等级
		/// </summary>
		public uint MaxLevel { get { return Last(this.id).level; } }

		/// <summary>
		/// 是否为普通攻击技能
		/// </summary>
		public bool IsBasic { get { return id == 1000 || id == 2000 || id == 3000; } }
		 
		/// <summary>
		/// 得到和给定职业相符合的所有技能ID列表
		/// </summary>
		/// <param name="profession"></param>
		/// <returns></returns>
		public static IEnumerable<uint> Where(Profession profession)
		{
			return
				from s in Table.Query<table.TableSkill>()
				where s.Profession == profession
				group s by s.id into g
				select g.Key;
		}

		public static TableSkill Where(uint id, uint level)
		{
			return (
				from s in Table.Query<table.TableSkill>()
				where s.id == id && s.level == level
				select s).FirstOrDefault();
		}

		/// <summary>
		/// 获得指定id的最低等级技能
		/// </summary>
		/// <param name="skillid"></param>
		/// <returns></returns>
		public static TableSkill First(uint skillid)
		{
			return (
				from s in Table.Query<table.TableSkill>()
				where s.id == skillid
				orderby s.level
				select s).First();
		}

		/// <summary>
		/// 获得指定id的最高等级技能
		/// </summary>
		/// <param name="skillid"></param>
		/// <returns></returns>
		public static TableSkill Last(uint skillid)
		{
			return (
				from s in Table.Query<table.TableSkill>()
				where s.id == skillid
				orderby s.level descending
				select s).First();
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
