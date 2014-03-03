using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace BWGame
{
	class WebSocket : GX.Net.WebSocket.IProxy, IDisposable
	{
		MessageWebSocket socket;
		DataWriter writer;
		readonly object syncRoot = new object();
		readonly Queue<byte[]> receiveQueue = new Queue<byte[]>();

		#region IProxy 成员

		public bool Connected { get; private set; }

		public void Open(string url)
		{
			receiveQueue.Clear();
			Dispose();

			try
			{
				socket = new MessageWebSocket();
				socket.Control.MessageType = SocketMessageType.Binary;
				socket.MessageReceived += (s, e) =>
				{
					try
					{
						using (var reader = e.GetDataReader())
						{
							var buf = new byte[reader.UnconsumedBufferLength];
							reader.ReadBytes(buf);

							if (e.MessageType == SocketMessageType.Binary)
							{
								Debug.WriteLine("WebSocket MessageReceived(binary): length=" + buf.Length);
								lock (syncRoot)
								{
									receiveQueue.Enqueue(buf);
								}
							}
							else
							{
								Debug.WriteLine("WebSocket MessageReceived(text): " + Encoding.UTF8.GetString(buf, 0, buf.Length));
							}
						}
					}
					catch (Exception ex)
					{
						Connected = false;
						var status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
						Debug.WriteLine("WebSocket MessageReceived Error: " + status);
					}
				};
				socket.Closed += (s, e) => Debug.WriteLine("WebSocket Closed");

				socket.ConnectAsync(new Uri(url)).AsTask().Wait();
				Connected = true;

				Debug.WriteLine("WebSocket Opened");
				writer = new DataWriter(socket.OutputStream);
			}
			catch (Exception ex)
			{
				Connected = false;
				var status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
				Debug.WriteLine("WebSocket Open Error: " + status);
			}
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

		public System.Collections.IEnumerator Run()
		{
			yield break;
		}

		public void Send(byte[] data)
		{
			writer.WriteBytes(data);
			try
			{
				writer.StoreAsync().AsTask().Wait();
			}
			catch (Exception ex)
			{
				Connected = false;
				var status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
				Debug.WriteLine("WebSocket Send Error: " + status);
			}
		}

		#endregion

		#region IDisposable 成员

		public void Dispose()
		{
			Connected = false;

			if (writer != null)
			{
				writer.Dispose();
				writer = null;
			}
			if (socket != null)
			{
				socket.Dispose();
				socket = null;
			}
		}

		#endregion
	}
}
