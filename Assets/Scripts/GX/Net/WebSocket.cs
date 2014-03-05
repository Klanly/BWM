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
		public interface IProxy
		{
			bool Connected { get; }
			void Open(string url);
			void Send(byte[] data);
			byte[] Receive();

			IEnumerator Run();
		}
		public static IProxy Proxy { get; set; }

		#region UniWeb WebSocket
#if !UNITY_WINRT || UNITY_EDITOR
		class UniWebWebSocket : IProxy
		{
			HTTP.WebSocket socket;
			readonly object syncRoot = new object();
			Queue<byte[]> queue;

			#region IProxy 成员
			public bool Connected { get { return socket.connected; } }

			public void Open(string url)
			{
				if (socket != null)
					socket.Close(HTTP.WebSocket.CloseEventCode.CloseEventCodeNormalClosure, string.Empty);
				queue = new Queue<byte[]>();
				socket = new HTTP.WebSocket();
				socket.OnBinaryMessageRecv += buf =>
				{
					lock (syncRoot)
					{
						queue.Enqueue(buf);
					}
				};

				// UniWeb can only work on http protocol. 
				// http://answers.unity3d.com/questions/575963/websocket-implementation-that-works-in-ide-and-on.html
				url = Regex.Replace(url, "^ws", "http");
				socket.Connect(url);
			}

			public void Send(byte[] data)
			{
				socket.Send(data);
			}

			public byte[] Receive()
			{
				if (queue == null)
					return null;
				lock (syncRoot)
				{
					if (queue.Count == 0)
						return null;
					return queue.Dequeue();
				}
			}

			public IEnumerator Run()
			{
				return socket.Dispatcher();
			}

			#endregion
		}
#endif
		#endregion

		#region WebSocket4Net WebSocket
#if !UNITY_WINRT || UNITY_EDITOR
		class WebSocket4NetProxy : IProxy
		{
			WebSocket4Net.WebSocket socket;
			readonly object syncRoot = new object();
			readonly Queue<byte[]> sendQueue = new Queue<byte[]>();
			readonly Queue<byte[]> receiveQueue = new Queue<byte[]>();
			
			
			#region IProxy 成员
			
			public bool Connected
			{
				get { return socket.State == WebSocket4Net.WebSocketState.Open; }
			}
			
			public void Open(string url)
			{
				sendQueue.Clear();
				receiveQueue.Clear();
				if (socket != null)
					socket.Close();
				socket = new WebSocket4Net.WebSocket(url);
				socket.DataReceived += (s, e) =>
				{
					//Debug.Log("WebSocket DataReceived: length=" + e.Data.Length);
					lock (syncRoot)
					{
						receiveQueue.Enqueue(e.Data);
					}
				};
				socket.Closed += (s, e) => Debug.Log("WebSocket Closed");
				socket.Opened += (s, e) => Debug.Log("WebSocket Opened");
				socket.Error += (s, e) => Debug.Log("WebSocket Error: " + e.Exception.Message);
				socket.MessageReceived += (s, e) => Debug.Log("WebSocket MessageReceived: " + e.Message);

				socket.Open();
			}
			
			public void Send(byte[] data)
			{
				if (Connected)
					socket.Send(data, 0, data.Length);
				else
					sendQueue.Enqueue(data);
			}
			
			public byte[] Receive()
			{
				if (receiveQueue == null)
					return null;
				lock (syncRoot)
				{
					if (receiveQueue.Count == 0)
						return null;
					return receiveQueue.Dequeue();
				}
			}
			
			public IEnumerator Run()
			{
				while (true)
				{
					yield return null;
					if (Connected && sendQueue.Count != 0)
					{
						socket.Send((from buf in sendQueue select new ArraySegment<byte>(buf)).ToList());
						sendQueue.Clear();
					}
				}
			}
			
			#endregion
		}
#endif
		#endregion

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

		/// <summary>
		/// 网络底层所需的收发轮询
		/// </summary>
		/// <returns></returns>
		public IEnumerator Run()
		{
			return Proxy.Run();
		}
	}
}