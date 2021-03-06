﻿using UnityEngine;
using System.Collections;
using System.Linq;

namespace Cmd
{
	#region Pos
	/// <summary>
	/// 游戏地图上的位置，单位为cm，原点在左下角。仅用于和服务器通信
	/// </summary>
	partial class Pos
	{
		public override string ToString()
		{
			return string.Format("Pos({0}cm, {1}cm)", x, y);
		}
	}
	#endregion

	#region ItemLocation
	partial class ItemLocation : System.IEquatable<ItemLocation>
	{
		#region Equatable
		public static bool operator ==(ItemLocation a, ItemLocation b)
		{
			if (System.Object.ReferenceEquals(a, b))
				return true;
			if (((object)a == null) || ((object)b == null))
				return false;
			return a.index == b.index && a.type == b.type;
		}
		public static bool operator !=(ItemLocation a, ItemLocation b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return this == obj as ItemLocation;
		}

		public override int GetHashCode()
		{
			return ((int)this.type << 16) | ((int)this.index & 0x0000FFFF);
		}

		#region IEquatable<ItemLocation> 成员

		public bool Equals(ItemLocation other)
		{
			return this == other;
		}

		#endregion

		#endregion

		public override string ToString()
		{
			return string.Format("{{{0},{1}}}", type, index);
		}
	}
	#endregion

	#region MapUserData
	partial class MapUserData
	{
		public static readonly MapUserData Empty = new MapUserData();

		public string GetRoleHeadSprite()
		{
			if (this.sexman)
				return "male";
			else
				return "female";
		}
	}
	#endregion

	#region SaveItem
	partial class SaveItem
	{
		#region Table cache
		private table.TableItem tableInfoCache;
		public table.TableItem TableInfo
		{
			get
			{
				if (tableInfoCache != null && tableInfoCache.id == this.baseid)
					return tableInfoCache;
				tableInfoCache = Table.Query<table.TableItem>().First(i => i.id == this.baseid);
				return tableInfoCache;
			}
		}
		#endregion

		public override string ToString()
		{
			return string.Format("#{0} {1}({2})x{3} @{4}",
				this.thisid, this.TableInfo.name, this.baseid, this.num, this.loc);
		}
	}
	#endregion

	#region SaveSkill
	partial class SaveSkill : System.IEquatable<SaveSkill>
	{
		#region Equatable
		public static bool operator ==(SaveSkill a, SaveSkill b)
		{
			if (System.Object.ReferenceEquals(a, b))
				return true;
			if (((object)a == null) || ((object)b == null))
				return false;
			return a.skillid == b.skillid && a.level == b.level;
		}
		public static bool operator !=(SaveSkill a, SaveSkill b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return this == obj as SaveSkill;
		}

		public override int GetHashCode()
		{
			return (int)(this.skillid << 16) | (int)this.level;
		}

		#region IEquatable<SaveSkill> Members

		public bool Equals(SaveSkill other)
		{
			return this == other;
		}

		#endregion

		#endregion

		#region Table cache
		private table.TableSkill tableInfoCache;
		public table.TableSkill TableInfo
		{
			get
			{
				if (tableInfoCache != null && tableInfoCache.id == this.skillid)
					return tableInfoCache;
				tableInfoCache = Table.Query<table.TableSkill>().First(i => i.id == this.skillid && i.level == this.level);
				return tableInfoCache;
			}
		}
		#endregion

		public override string ToString()
		{
			return TableInfo.ToString();
		}
	}
	#endregion

	#region SaveBuff
    partial class SaveBuff : System.IEquatable<SaveBuff>
    {
        #region Equatable
        public static bool operator ==(SaveBuff a, SaveBuff b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;
            if (((object)a == null) || ((object)b == null))
                return false;
            return a.buffid == b.buffid && a.level == b.level;
        }
        public static bool operator !=(SaveBuff a, SaveBuff b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return this == obj as SaveBuff;
        }

        public override int GetHashCode()
        {
            return (int)(this.buffid << 16) | (int)this.level;
        }

        #region IEquatable<SaveBuff> Members

        public bool Equals(SaveBuff other)
        {
            return this == other;
        }

        #endregion

        #endregion

        #region Table cache
        private table.TableBuff tableInfoCache;
        public table.TableBuff TableInfo
        {
            get
            {
                if (tableInfoCache != null && tableInfoCache.id == this.buffid)
                    return tableInfoCache;
                tableInfoCache = Table.Query<table.TableBuff>().First(i => i.id == this.buffid && i.level == this.level);
                return tableInfoCache;
            }
        }
        #endregion
        
        public override string ToString()
		{
			this.bitmask.ToString();
			return string.Format("#{0} level:{1} time:{2} value:{3} bitmask:{4}", this.buffid, this.level, this.time, this.value, this.bitmask.ToBitString());
		}
	}
	#endregion

	#region Quest
	partial class SaveQuest
	{
		public override string ToString()
		{
			return string.Format("#{0}, {1}/{2}", questid, stepcur, stepall);
		}
	}

	partial class QuestTrace
	{
		/// <summary>
		/// 任务追踪描述信息
		/// </summary>
		public string TraceContent
		{
			get
			{
				try
				{
					return string.Format(desc, squest.questid, squest.stepcur, squest.stepall);
				}
				catch (System.Exception ex)
				{
					Debug.LogError("Quest content format error: " + ex.Message + "\n" + this);
					return desc;
				}
			}
		}
		public override string ToString()
		{
			return string.Format("{0}, desc={1}", squest, desc);
		}
	}
	#endregion
}

static class CommonExtensions
{
	/// <summary>
	/// 职业的中文名称
	/// </summary>
	/// <param name="profession"></param>
	/// <returns></returns>
	public static string GetName(this Cmd.Profession profession)
	{
		switch (profession)
		{
			case Cmd.Profession.Profession_ZhanShi: return "战士";
			case Cmd.Profession.Profession_DaoShi: return "道士";
			case Cmd.Profession.Profession_FaShi: return "法师";
			default: return profession.ToString();
		}
	}

	public static GameObject GetGameObject(this Cmd.SceneEntryUid entry)
	{
		if (entry == null)
			return null;
		switch (entry.entrytype)
		{
			case Cmd.SceneEntryType.SceneEntryType_Npc:
				{
					var target = Npc.All[entry.entryid];
					if (target != null)
						return target.gameObject;
				}
				break;
			case Cmd.SceneEntryType.SceneEntryType_Player:
				{
					var target = Role.All[entry.entryid];
					if (target != null)
						return target.gameObject;
				}
				break;
			default:
				break;
		}
		return null;
	}
}