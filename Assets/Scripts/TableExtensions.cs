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
	}
}

public static class TableExtensions
{
}
