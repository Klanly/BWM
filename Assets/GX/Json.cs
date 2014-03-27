using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GX
{
	public class Json
	{
		public interface IProxy
		{
			T Deserialize<T>(string value);
			string Serialize(object value);
		}

#if !UNITY_WP8 || UNITY_EDITOR
		class JsonFxProxy : IProxy
		{
			#region IProxy 成员

			public T Deserialize<T>(string value)
			{
				return JsonFx.Json.JsonReader.Deserialize<T>(value);
			}

			public string Serialize(object value)
			{
				return JsonFx.Json.JsonWriter.Serialize(value);
			}

			#endregion
		}
#endif

		public static IProxy Proxy { get; set; }

		static Json()
		{
#if !UNITY_WP8 || UNITY_EDITOR
			Proxy = new JsonFxProxy();
#endif
		}

		public static T Deserialize<T>(string value) { return Proxy.Deserialize<T>(value); }
		public static string Serialize(object value) { return Proxy.Serialize(value); }
	}
}