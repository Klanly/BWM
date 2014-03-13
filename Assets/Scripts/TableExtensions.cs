// 表格相关的扩充代码
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cmd;

namespace table
{
	partial class TableMapItem
	{
		public static TableMapItem Select(uint id)
		{
			return (from i in Table.Query<TableMapItem>() where i.id == id select i).First();
		}
	}

	partial class TableAvatarItem
	{
		public bool sexman { get { return this.sex == 1; } }

		public static TableAvatarItem Select(Profession profession, bool sexman)
		{
			var pro = (uint)profession;
			var sex = sexman ? 1 : 2;
			return (from i in Table.Query<TableAvatarItem>() where i.profession == pro && i.sex == sex select i).First();
		}
	}
}

public static class TableExtensions
{
}
