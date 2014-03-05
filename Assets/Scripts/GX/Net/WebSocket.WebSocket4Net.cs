using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !UNITY_WINRT || UNITY_EDITOR
class WebSocket4NetProxy : GX.Net.WebSocket.IProxy
{
	WebSocket4Net.WebSocket socket;
	readonly object syncRoot = new object();
	readonly Queue<byte[]> receiveQueue = new Queue<byte[]>();

	#region IProxy 成员
	public Action OnOpen { get; set; }
	public Action OnError { get; set; }
	public Action OnClose { get; set; }
	
	public void Open(string url)
	{
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
		socket.Closed += (s, e) =>
		{
			Debug.Log("WebSocket Closed");
			if(OnClose != null)
				OnClose();
		};
		socket.Opened += (s, e) =>
		{
			Debug.Log("WebSocket Opened");
			if (OnOpen != null)
				OnOpen();
		};
		socket.Error += (s, e) =>
		{
			Debug.Log("WebSocket Error: " + e.Exception.Message);
			if (OnError != null)
				OnError();
		};
		socket.MessageReceived += (s, e) => Debug.Log("WebSocket MessageReceived: " + e.Message);

		socket.Open();
	}

	public void Send(byte[] data)
	{
		socket.Send(data, 0, data.Length);
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

	#endregion
}
#endif