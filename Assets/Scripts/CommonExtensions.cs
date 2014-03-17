using UnityEngine;
using System.Collections;

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

	partial class MapUserData
	{
		public static readonly MapUserData Empty = new MapUserData();
	}
}

static class CommonExtensions
{
}