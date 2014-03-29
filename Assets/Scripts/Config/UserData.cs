using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using GX;

namespace Config
{
	partial class UserData
	{
		public static UserData Instance { get; private set; }

		static UserData()
		{
			ProtoBuf.Serializer.PrepareSerializer<UserData>();
			Instance = Deserialize();
			Instance.PropertyChanged += Serialize;
		}

		public static void Reset()
		{
			PlayerPrefs.DeleteAll();
			Instance = new UserData();
			Instance.PropertyChanged += Serialize;
		}

		public static void Serialize(object sender = null, System.ComponentModel.PropertyChangedEventArgs e = null)
		{
			Serialize(Instance);
		}

		static void Serialize(ProtoBuf.IExtensible message)
		{
			PlayerPrefs.SetInt("Version", 1);
			byte[] buf = null;
			using(var mem = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize(mem, message);
				buf = mem.ToArray();
			}
			PlayerPrefs.SetString("UserData", Convert.ToBase64String(Crypto.Encode(buf)));
			Debug.Log("Serialize: " + message.ToStringDebug());
		}

		static UserData Deserialize()
		{
			if (PlayerPrefs.GetInt("Version") == 1)
			{
				try
				{
					var buf = Crypto.Decode(Convert.FromBase64String(PlayerPrefs.GetString("UserData")));
					using(var mem = new MemoryStream(buf))
					{
						return ProtoBuf.Serializer.Deserialize<UserData>(mem);
					}
				}
				catch (Exception ex)
				{
					Debug.Log("Deserialize error and reset: " + ex.Message);
				}
			}
			return new UserData();
		}
	}
}