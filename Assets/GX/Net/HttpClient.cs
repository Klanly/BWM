using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GX
{
	public class HttpClient
	{
		public string UID { get; set; }
		public string SID { get; set; }
		public int GameID { get; set; }
		public int ZoneID { get; set; }
		public string LoginUrl { get; set; }
		public string GatewayUrl { get; private set; }

		private WWW Send(string url, string action, Dictionary<string, object> message)
		{
			var cmd = new Dictionary<string, object>();
			cmd["do"] = action;
			cmd["data"] = message;
			if (!string.IsNullOrEmpty(UID))
				cmd["uid"] = UID;
			if (GameID != 0)
				cmd["gameid"] = GameID;
			if (ZoneID != 0)
				cmd["zoneid"] = ZoneID;
			var json = GX.Json.Serialize(cmd);

			if (!string.IsNullOrEmpty(SID))
				url += "?smd=md5&sign=" + GX.MD5.ComputeHashString(json + SID);
			var www = new WWW(url, GX.Encoding.GetBytes(json));
			//Debug.Log("Send:\n" + url + "\n" + json);
			return www;
		}

		public IEnumerator Send(string action, Dictionary<string, object> message, Action<WWW> callback = null)
		{
			#region register-newaccount
			if (string.IsNullOrEmpty(UID) || string.IsNullOrEmpty(SID))
			{
				/*
				http://14.17.104.56:7000/httplogin
				{"do":"register-newaccount","data":{"mid":"839faa337c04fcf11ae77180bd33f1536f6e39ed"},"gameid":304,"zoneid":301}
				*/
				var www = Send(LoginUrl, "register-newaccount", new Dictionary<string, object> { { "mid", SystemInfo.deviceUniqueIdentifier } });
				yield return www;
				var response = new HttpResponse(www);
				if (response.IsError)
				{
					Debug.LogError(string.Format("[WWW] ERROR {0} {1}\n{2}", LoginUrl, www.error, www.text));
					yield break;
				}

				/*
				{"data":{"mid":"839faa337c04fcf11ae77180bd33f1536f6e39ed","sid":"9dfbae7eb7472f71134ec21bc65efe3d","uid":"114701"},"do":"register-newaccount","gameid":304,"zoneid":301}
				*/
				int number = 0;
				if (int.TryParse(response["gameid"].ToString(), out number))
					this.GameID = number;
				if (int.TryParse(response["zoneid"].ToString(), out number))
					this.ZoneID = number;
				this.UID = response.Data<string>("uid");
				this.SID = response.Data<string>("sid");
			}
			#endregion

			#region get-zone-gatewayurl
			if (string.IsNullOrEmpty(GatewayUrl))
			{
				/*
				http://14.17.104.56:7000/httplogin?smd=md5&sign=1837f72e72032f7ba2ef4a7f0a6ca8f9
				{"do":"get-zone-gatewayurl","data":{"gameid":304,"zoneid":301},"uid":"114701","gameid":304,"zoneid":301}
				*/
				var www = Send(LoginUrl, "get-zone-gatewayurl", new Dictionary<string, object> { { "gameid", this.GameID }, { "zoneid", this.ZoneID } });
				yield return www;
				var response = new HttpResponse(www);
				if (response.IsError)
				{
					Debug.LogError(string.Format("[WWW] ERROR {0} {1}\n{2}", LoginUrl, www.error, www.text));
					yield break;
				}

				/*
				{"data":{"gameid":304,"gatewayurl":"http://14.17.104.56:7001/shen/user/http","zoneid":301},"do":"get-zone-gatewayurl","errno":"0","gameid":304,"st":1416965726,"uid":"114699","zoneid":301}
				*/
				int number = 0;
				if (int.TryParse(response.Data("gameid").ToString(), out number))
					this.GameID = number;
				if (int.TryParse(response.Data("zoneid").ToString(), out number))
					this.ZoneID = number;
				this.GatewayUrl = response.Data<string>("gatewayurl");
			}
			#endregion

			{
				var www = Send(GatewayUrl, action, message);
				yield return www;
				if (!string.IsNullOrEmpty(www.error))
				{
					Debug.LogError(string.Format("[WWW] ERROR {0} {1}\n{2}", GatewayUrl, www.error, www.text));
					yield break;
				}
				if (callback != null)
					callback(www);
			}
		}

		public IEnumerator Send(HttpRequest request, Action<HttpResponse> callback = null)
		{
			if (callback == null)
				return Send(request.Action, request.Data);
			else
				return Send(request.Action, request.Data, www => callback(new GX.HttpResponse(www)));
		}
	}
}
