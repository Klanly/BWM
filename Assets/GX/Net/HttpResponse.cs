using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GX
{
	public class HttpResponse
	{
		public bool IsError { get { return !string.IsNullOrEmpty(ErrorMessage); } }
		public string ErrorMessage { get; private set; }

		public object this[string key]
		{
			get
			{
				if(this.response != null)
				{
					object value;
					if(this.response.TryGetValue(key, out value))
						return value;
				}
				return null;
			} 
		}
		public object Data(string key)
		{
			if (this.data != null)
			{
				object value;
				if (this.data.TryGetValue(key, out value))
					return value;
			}
			return null;
		}
		public T Data<T>(string key)
		{
			var ret = this.Data(key);
			return ret != null ? (T)ret : default(T);
		}

		private string json;
		private Dictionary<string, object> response;
		private Dictionary<string, object> data;

		public HttpResponse(string json)
		{
			Create(json);
		}

		public HttpResponse(WWW www)
		{
			if (www == null)
			{
				this.ErrorMessage = "WWW is null";
				return;
			}
			if (!string.IsNullOrEmpty(www.error))
			{
				this.json = www.text;
				this.ErrorMessage = www.error;
				return;
			}
			Create(www.text);
		}

		private void Create(string json)
		{
			this.json = json;
			this.ErrorMessage = "Unknown";
			try
			{
				this.response = GX.Json.Deserialize<Dictionary<string, object>>(this.json);
				if (this.response != null)
				{
					var errno = this["errno"];
					if (errno != null && errno.ToString() != "0")
					{
						this.ErrorMessage = errno.ToString();
						return;
					}
					this.data = this["data"] as Dictionary<string, object>;
					errno = this.Data("errno");
					if (errno != null && errno.ToString() != "0")
					{
						this.ErrorMessage = errno.ToString();
						return;
					}
				}
				this.ErrorMessage = string.Empty;
			}
			catch (Exception ex)
			{
				this.ErrorMessage = ex.Message;
				Debug.LogException(ex);
			}
		}

		public override string ToString()
		{
			return json;
		}
	}
}
