using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZuo
{
	class WebSocket : GX.Net.WebSocket.IProxy
	{
		WebSocket4Net.WebSocket socket;
		readonly object syncRoot = new object();
		readonly Queue<byte[]> sendQueue = new Queue<byte[]>();
		readonly Queue<byte[]> receiveQueue = new Queue<byte[]>();

		#region IProxy 成员

		public bool Connected { get { return socket.State == WebSocket4Net.WebSocketState.Open; } }

		public void Open(string url)
		{
			sendQueue.Clear();
			receiveQueue.Clear();

			socket = new WebSocket4Net.WebSocket(url);
			if (socket != null)
					socket.Close();

			socket.DataReceived += (s, e) =>
			{
				Debug.WriteLine("WebSocket DataReceived: length=" + e.Data.Length);
				lock (syncRoot)
				{
					receiveQueue.Enqueue(e.Data);
				}
			};
			socket.Closed += (s, e) => Debug.WriteLine("WebSocket Closed");
			socket.Opened += (s, e) => Debug.WriteLine("WebSocket Opened");
			socket.Error += (s, e) => Debug.WriteLine("WebSocket Error: " + e.Exception.Message);
			socket.MessageReceived += (s, e) => Debug.WriteLine("WebSocket MessageReceived: " + e.Message);

			socket.Open();
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

		public void Send(byte[] data)
		{
			if (Connected)
				socket.Send(data, 0, data.Length);
			else
				sendQueue.Enqueue(data);
		}

		#endregion
	}
}
