using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using GX;

public static class Extensions
{
	#region Random
	private static readonly System.Random random = new System.Random();

	/// <summary>
	/// 得到随机的bool值
	/// </summary>
	/// <param name="random"></param>
	/// <param name="successRate">胜率，返回结果有该概率为true</param>
	/// <returns></returns>
	public static bool Next(this System.Random random, double successRate)
	{
		return successRate > random.NextDouble();
	}

	/// <summary>
	/// 从序列中随机选择一个元素
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <returns>失败返回<c>default(T)</c></returns>
	public static T Random<T>(this IList<T> list)
	{
		if (list == null || list.Count == 0)
			return default(T);
		return list[random.Next(list.Count)];
	}
	#endregion

	#region Enumerable
	public static IEnumerable<object> AsEnumerable(this IEnumerator it)
	{
		if (it == null)
			yield break;
		while (it.MoveNext())
			yield return it.Current;
	}

	public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> it)
	{
		if (it == null)
			yield break;
		while (it.MoveNext())
			yield return it.Current;
	}

	public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dic, IEnumerable<KeyValuePair<TKey, TValue>> collection)
	{
		foreach (var d in collection)
			dic.Add(d.Key, d.Value);
	}

#if UNITY_METRO && !UNITY_EDITOR
	public static void ForEach<T>(this List<T> list, Action<T> action)
	{
		foreach (var i in list)
			action(i);
	}
#endif
	#endregion

	#region Stream
	public static byte[] ReadAllBytes(this Stream stream)
	{
		int offset = 0;
		int count = (int)stream.Length;
		var buffer = new byte[count];
		while (count > 0)
		{
			int n = stream.Read(buffer, offset, count);
			if (n == 0)
				throw new EndOfStreamException();
			offset += n;
			count -= n;
		}
		return buffer;
	}
	#endregion

	#region Diagnostics
	/// <summary>
	/// 得到所有公有字段的值，用于调试
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static string ToStringDebug(this object obj)
	{
		return "{ " + string.Join(", ", (
			from f in obj.GetType().GetRuntimeFields()
			select f.Name + "=" + f.GetValue(obj)).ToArray()) + " }";
	} 
	#endregion
}
