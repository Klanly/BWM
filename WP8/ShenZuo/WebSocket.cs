using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZuo
{
	class WebSocket : GX.Net.WebSocket.IProxy
	{
		WebSocket4Net.WebSocket socket;
		readonly object syncRoot = new object();
		Queue<byte[]> queue;

		#region IProxy 成员

		public void Open(string url)
		{
			queue = new Queue<byte[]>();
			socket = new WebSocket4Net.WebSocket(url);
			socket.DataReceived += (s, e) =>
			{
				lock (syncRoot)
				{
					queue.Enqueue(e.Data);
				}
			};
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
			yield break;
		}

		public void Send(byte[] data)
		{
			socket.Send(data, 0, data.Length);
		}

		#endregion
	}
}
