#if (!UNITY_WP8 && !UNITY_WINRT) || UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace GX
{
	class JsonJsonFx : Json.IProxy
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
}
#endif