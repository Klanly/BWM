using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace GX.Net
{
	/// <summary>
	/// 跨平台的WebSocket实现
	/// </summary>
	/// <remarks>
	/// TODO: 若需要多个实例，可以考虑对<see cref="WebSocket.IProxy"/>实现工厂
	/// </remarks>
	public static class WebSocket
	{
		public enum State
		{
			None = -1,
			Connecting = 0,
			Open = 1,
			Closing = 2,
			Closed = 3,
		}

		#region Proxy
		/// <summary>
		/// 接口定义参考：http://www.w3.org/TR/2011/CR-websockets-20111208/#the-websocket-interface
		/// </summary>
		public interface IProxy
		{
			GX.Net.WebSocket.State State { get; }
			void Open(string url);
			void Close();
			void Send(byte[] data);
			byte[] Receive();
		}
		public static IProxy Proxy { get; set; }

		static WebSocket()
		{
#if !UNITY_WINRT || UNITY_EDITOR
			Proxy = new WebSocket4NetProxy();
#endif
		}
		#endregion

		private static readonly MessageSerializer serizlizer = new MessageSerializer();

		public static void Open(string url = "ws://echo.websocket.org")
		{
			Debug.Log("WebSocket to: " + url);
			Proxy.Open(url);
		}

		public static void Send(ProtoBuf.IExtensible msg)
		{
			if (msg.GetType() != typeof(Pmd.TickRequestNullUserPmd_CS) &&
				msg.GetType() != typeof(Pmd.TickReturnNullUserPmd_CS))
				Debug.Log("<color=green>[SEND]</color>" + msg.Dump());

			var buf = serizlizer.Serialize(msg);
			Proxy.Send(buf);
		}

		public static void Send(params ProtoBuf.IExtensible[] message)
		{
			var buf = serizlizer.Serialize(message);
			Proxy.Send(buf);
		}

		public static IEnumerable<ProtoBuf.IExtensible> Receive()
		{
			var buf = Proxy.Receive();
			if (buf == null)
				yield break;
			using (var mem = new MemoryStream(buf))
			{
				while (mem.Position < mem.Length)
				{
					var msg = serizlizer.Deserialize(mem);
					if (msg == null)
						continue;
					if (msg.GetType() != typeof(Pmd.TickRequestNullUserPmd_CS) &&
						msg.GetType() != typeof(Pmd.TickReturnNullUserPmd_CS) &&
						msg.GetType() != typeof(Cmd.SetUserHpSpDataUserCmd_S) &&
						msg.GetType() != typeof(Cmd.ChangeUserHpDataUserCmd_S) &&
						msg.GetType() != typeof(Cmd.AddMapUserDataAndPosMapUserCmd_S) &&
						msg.GetType() != typeof(Cmd.AddMapNpcDataAndPosMapUserCmd_S))
						Debug.Log("<color=yellow>[RECV]</color>" + msg.Dump());
					yield return msg;
				}
			}
		}
	}
}