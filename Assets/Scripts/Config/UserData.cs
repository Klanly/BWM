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
		/// <summary>
		/// 触发<see cref="PropertyChanged"/>事件
		/// </summary>
		/// <param name="propertyName"></param>
		public void FirePropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(propertyName);
		}

		public static UserData Instance { get; private set; }

		static UserData()
		{
			ProtoBuf.Serializer.PrepareSerializer<UserData>();
			Instance = Deserialize();
			Instance.Init();
		}

		public static void Reset()
		{
			PlayerPrefs.DeleteAll();
			Instance = new UserData();
			Instance.Init();
		}

		static void Serialize(object sender = null, System.ComponentModel.PropertyChangedEventArgs e = null)
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
			Debug.Log("Serialize: " + message.Dump());
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

		/// <summary>
		/// 档案加载后的预处理和规整
		/// </summary>
		private void Init()
		{
			this.skillbar.Resize<uint>(SkillManager.FireThumbsCount);
			this.PropertyChanged += Serialize;
		}
	}
}