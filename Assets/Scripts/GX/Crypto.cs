using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace GX
{
	public class Crypto
	{
		public interface IProxy
		{
			void SetKey(byte[] key, byte[] iv);
			byte[] Encode(byte[] buf);
			byte[] Decode(byte[] buf);
		}

		public static IProxy Proxy { get; set; }

#if !UNITY_WINRT || UNITY_EDITOR
		class AesProxy : IProxy
		{
			private readonly SymmetricAlgorithm algorithm = new System.Security.Cryptography.AesManaged() { KeySize = 128 };
			private SecretBytes IV;
			private SecretBytes KEY;

			#region IProxy 成员

			public void SetKey(byte[] key, byte[] iv)
			{
				KEY = new SecretBytes() { Bytes = key };
				IV = new SecretBytes() { Bytes = iv };
			}

			public byte[] Encode(byte[] buf)
			{
				using (var mem = new MemoryStream())
				using (var crypto = new CryptoStream(mem, algorithm.CreateEncryptor(KEY.Bytes, IV.Bytes), CryptoStreamMode.Write))
				{
					crypto.Write(buf, 0, buf.Length);
					crypto.FlushFinalBlock();
					crypto.Close();
					return mem.ToArray();
				}
			}

			public byte[] Decode(byte[] buf)
			{
				using (var mem = new MemoryStream())
				using (var crypto = new CryptoStream(mem, algorithm.CreateDecryptor(KEY.Bytes, IV.Bytes), CryptoStreamMode.Write))
				{
					crypto.Write(buf, 0, buf.Length);
					crypto.FlushFinalBlock();
					crypto.Close();
					return mem.ToArray();
				}
			}

			#endregion
		}

#endif
		
		static Crypto()
		{
#if !UNITY_WINRT || UNITY_EDITOR
			Proxy = new AesProxy();
#endif
		}

		public static void SetKey(byte[] key = null, byte[] iv = null)
		{
			Proxy.SetKey(
				key ?? GX.MD5.ComputeHash(Encoding.GetBytes(SystemInfo.deviceUniqueIdentifier)), 
				iv ?? new byte[128]);
		}

		public static byte[] Encode(byte[] buf)
		{
			return Proxy.Encode(buf);
		}

		public static byte[] Decode(byte[] buf)
		{
			return Proxy.Decode(buf);
		}
	}
}