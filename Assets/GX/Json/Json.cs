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

		public static IProxy Proxy { get; set; }

		static Json()
		{
#if (!UNITY_WP8 && !UNITY_WINRT) || UNITY_EDITOR
			Proxy = new JsonJsonFx();
#endif
		}

		public static T Deserialize<T>(string value) { return Proxy.Deserialize<T>(value); }
		public static string Serialize(object value) { return Proxy.Serialize(value); }
	}
}