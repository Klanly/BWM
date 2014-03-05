using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace GX.Net
{
	public class WebSocket
	{
		#region Proxy
		/// <summary>
		/// 接口定义参考：http://www.w3.org/TR/2011/CR-websockets-20111208/#the-websocket-interface
		/// </summary>
		public interface IProxy
		{
			Action OnOpen { get; set; }
			Action OnError { get; set; }
			Action OnClose { get; set; }
			void Open(string url);
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

		private readonly MessageSerializer serizlizer = new MessageSerializer();

		public void Open(string url = "ws://echo.websocket.org")
		{
			Debug.Log("WebSocket to: " + url);
			Proxy.Open(url);
		}

		public void Send(ProtoBuf.IExtensible message)
		{
			Debug.Log("[SEND]" + message.ToStringDebug());
			var buf = serizlizer.Serialize(message);
			Proxy.Send(buf);
		}

		public void Send(params ProtoBuf.IExtensible[] message)
		{
			var buf = serizlizer.Serialize(message);
			Proxy.Send(buf);
		}

		public IEnumerable<ProtoBuf.IExtensible> Receive()
		{
			var buf = Proxy.Receive();
			if (buf == null)
				yield break;
			using (var mem = new MemoryStream(buf))
			{
				while (mem.Position < mem.Length)
				{
					var msg = serizlizer.Deserialize(mem);
					Debug.Log("[RECV]" + msg.ToStringDebug());
					yield return msg;
				}
			}
		}
	}
}