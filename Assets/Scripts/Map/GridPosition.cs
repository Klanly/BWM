using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏地图上的逻辑格子位置
/// 原点在屏幕左下角
/// </summary>
public struct GridPosition
{
	/// <summary>
	/// 水平
	/// </summary>
	public int X;
	/// <summary>
	/// 竖直
	/// </summary>
	public int Z;

	public override string ToString()
	{
		return string.Format("{{0},{1}}", X, Z);
	}
}
