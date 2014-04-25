using UnityEngine;
using System.Collections;

public class MapGrid : System.IEquatable<MapGrid>
{
	/// <summary>
	/// 每个格子<see cref="MapGrid"/>宽度
	/// </summary>
	public const float Width = 0.25f;
	/// <summary>
	/// 每个格子<see cref="MapGrid"/>高度
	/// </summary>
	public const float Height = Width;

	public int x;
	public int y;

	#region Equatable
	public static bool operator ==(MapGrid a, MapGrid b)
	{
		if (System.Object.ReferenceEquals(a, b))
			return true;
		if (((object)a == null) || ((object)b == null))
			return false;
		return a.x == b.x && a.y == b.y;
	}
	public static bool operator !=(MapGrid a, MapGrid b)
	{
		return !(a == b);
	}

	public override bool Equals(object obj)
	{
		return obj is MapGrid ? this == (MapGrid)obj : false;
	}

	public override int GetHashCode()
	{
		return (int)(this.x ^ this.y);
	}

	#region IEquatable<Pos> Members

	public bool Equals(MapGrid other)
	{
		return this == other;
	}

	#endregion

	#endregion

	public override string ToString()
	{
		return string.Format("Grid({0}, {1})", x, y);
	}

	public static implicit operator Cmd.Pos(MapGrid grid)
	{
		return new Cmd.Pos() { x = grid.x, y = grid.y };
	}
}
