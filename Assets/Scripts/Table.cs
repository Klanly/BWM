using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public static class Table
{
	class TypeCompare : Comparer<Type>
	{
		public override int Compare(Type x, Type y)
		{
			return x.FullName.CompareTo(y.FullName);
		}
	}

	private static readonly SortedDictionary<Type, WeakReference> cache = new SortedDictionary<Type, WeakReference>(new TypeCompare());


	public static IList<T> Query<T>(string path = null) where T : ProtoBuf.IExtensible
	{
		WeakReference wr;
		if (cache.TryGetValue(typeof(T), out wr) == false)
			cache[typeof(T)] = wr = new WeakReference(null);
		var ret = wr.Target as IList<T>;
		if (ret == null)
		{
			if (string.IsNullOrEmpty(path))
				path = "Table/" + typeof(T).Name;

			wr.Target = ret = new List<T>(Load<T>(path));
		}
		return ret;
	}

	static IEnumerable<T> Load<T>(string path) where T : ProtoBuf.IExtensible
	{
		using (var mem = new MemoryStream(Resources.Load<TextAsset>(path).bytes))
		{
			// 忽略前面的 google.protobuf.FileDescriptorSet 元数据定义信息
			int descriptor;
			if (ProtoBuf.Serializer.TryReadLengthPrefix(mem, ProtoBuf.PrefixStyle.Base128, out descriptor))
			{
				mem.Seek(descriptor, SeekOrigin.Current);
				while (mem.Position < mem.Length)
				{
					yield return ProtoBuf.Serializer.DeserializeWithLengthPrefix<T>(mem, ProtoBuf.PrefixStyle.Base128);
				}
			}
		}
	}

}
