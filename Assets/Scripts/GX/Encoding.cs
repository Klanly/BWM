using UnityEngine;
using System.Collections;

namespace GX
{
	public class Encoding
	{
		public interface IProxy
		{
			byte[] GetBytes(string s);
			string GetString(byte[] bytes);
		}

#if (!UNITY_WP8 && !UNITY_WINRT) || UNITY_EDITOR

		class DefaultEncoding : IProxy
		{
			private readonly System.Text.Encoding encoding;
			public DefaultEncoding(System.Text.Encoding encoding = null) { this.encoding = encoding ?? System.Text.Encoding.UTF8; }

			#region IProxy 成员

			public byte[] GetBytes(string s) { return encoding.GetBytes(s); }
			public string GetString(byte[] bytes) { return encoding.GetString(bytes); }

			#endregion
		}

#endif

		public static IProxy Proxy { get; set; }

		static Encoding()
		{
#if (!UNITY_WP8 && !UNITY_WINRT) || UNITY_EDITOR
			Proxy = new DefaultEncoding();
#endif
		}

		public static byte[] GetBytes(string s) { return Proxy.GetBytes(s); }
		public static string GetString(byte[] bytes) { return Proxy.GetString(bytes); }
	}
}