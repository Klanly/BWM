// 表格相关的扩充代码
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace table
{
	partial class TableMapItem
	{
		public static TableMapItem QueryID(uint id)
		{
			return (from i in Table.Query<TableMapItem>() where i.id == id select i).First();
		}
	}
}

public static class TableExtensions
{
}
