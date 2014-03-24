using UnityEngine;
using System.Collections;
using System.Linq;

namespace Cmd
{
	#region Pos
	/// <summary>
	/// 游戏地图上的逻辑格子位置
	/// 原点在屏幕左下角
	/// </summary>
	partial class Pos : System.IEquatable<Pos>
	{
		public static bool operator ==(Pos a, Pos b)
		{
			if (System.Object.ReferenceEquals(a, b))
				return true;
			if (((object)a == null) || ((object)b == null))
				return false;
			return a.x == b.x && a.y == b.y;
		}
		public static bool operator !=(Pos a, Pos b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return this == obj as Pos;
		}

		public override int GetHashCode()
		{
			return (int)(this.x ^ this.y);
		}

		#region IEquatable<Pos> Members

		public bool Equals(Pos other)
		{
			return this == other;
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{{{0},{1}}}", x, y);
		}

	}
	#endregion

	#region ItemLocation
	partial class ItemLocation : System.IEquatable<ItemLocation>
	{
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
	}
	#endregion

	#region SaveItem
	partial class SaveItem
	{
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

		public override string ToString()
		{
			return string.Format("#{0} {1}({2})x{3} @{4}", 
				this.thisid, this.TableInfo.name, this.baseid, this.num, this.loc);
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
}