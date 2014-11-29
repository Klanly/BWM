using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HttpRequest
{
	public string Action { get; set; }
	public Dictionary<string, object> Data { get; private set; }

	public HttpRequest() { }
	public HttpRequest(string action, IDictionary<string, object> data = null)
	{
		this.Action = action;
		if (data != null)
			this.Data = new Dictionary<string, object>(data);
	}

	public object this[string key]
	{
		get
		{
			if (Data != null)
			{
				object value;
				if (Data.TryGetValue(key, out value))
					return value;
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				if (Data == null)
					return;
				Data.Remove(key);
			}
			else
			{
				if (Data == null)
					Data = new Dictionary<string, object>();
				Data[key] = value;
			}
		}
	}

	public override string ToString()
	{
		return Action + " -> " + GX.Json.Serialize(Data);
	}
}
