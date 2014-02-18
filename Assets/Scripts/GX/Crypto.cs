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
		private static readonly SymmetricAlgorithm algorithm = new System.Security.Cryptography.AesManaged() { KeySize = 128 };
		private static readonly SecretBytes IV;
		private static readonly SecretBytes KEY;

		static Crypto()
		{
			KEY = new SecretBytes() { Bytes = GX.MD5.ComputeHash(Encoding.GetBytes(SystemInfo.deviceUniqueIdentifier + algorithm.GetType().FullName)) };
			IV = new SecretBytes() { Bytes = GX.MD5.ComputeHash(Encoding.GetBytes(SystemInfo.deviceUniqueIdentifier)) };
		}

		public static byte[] Encode(byte[] buf)
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

		public static byte[] Decode(byte[] buf)
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
	}
}