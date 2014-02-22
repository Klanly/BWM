using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace GX.Net
{
	/// <summary>
	/// <see cref="ProtoBuf.IExtensible"/>对象和字节流间的编码器
	/// </summary>
	/// <remarks>性能分析参见：http://www.servicestack.net/benchmarks/NorthwindDatabaseRowsSerialization.100000-times.2010-08-17.html </remarks>
	public class MessageSerializer
	{
		private readonly Dictionary<MessageType, Func<Stream, ProtoBuf.IExtensible>> deserializeTable = new Dictionary<MessageType, Func<Stream, ProtoBuf.IExtensible>>();
		private readonly Dictionary<Type, MessageType> messageTypeTable = new Dictionary<Type, MessageType>();

		#region Serialize
		public byte[] Serialize(ProtoBuf.IExtensible message)
		{
			using (var mem = new MemoryStream())
			{
				SerializeTo(mem, message);
				return mem.ToArray();
			}
		}

		public byte[] Serialize(IEnumerable<ProtoBuf.IExtensible> message)
		{
			using (var mem = new MemoryStream())
			{
				foreach (var m in message)
					SerializeTo(mem, m);
				return mem.ToArray();
			}
		}

		public void SerializeTo(Stream stream, ProtoBuf.IExtensible message)
		{
			Debug.Assert(ProtoBuf.Serializer.NonGeneric.CanSerialize(message.GetType()));

			var messageType = messageTypeTable[message.GetType()];
			messageType.Serialize(stream);

#if false
			// 以下方法实现中未能正确处理泛型类型探测，此处的T全部以ProtoBuf.IExtensible对待了。
			// 若有性能不足，以后可以仿deserializeTable的方式，缓存加速
			ProtoBuf.Serializer.Serialize(dest, message);
#else
			//ProtoBuf.Serializer.NonGeneric.Serialize(stream, message);
			ProtoBuf.Serializer.NonGeneric.SerializeWithLengthPrefix(stream, message, ProtoBuf.PrefixStyle.Base128, 0);
#endif
		}

		#endregion

		#region Deserialize

		public ProtoBuf.IExtensible Deserialize(byte[] messagePackageData)
		{
			return Deserialize(new MemoryStream(messagePackageData, 0, messagePackageData.Length));
		}

		public ProtoBuf.IExtensible Deserialize(byte[] messagePackageData, int offset, int count)
		{
			return Deserialize(new MemoryStream(messagePackageData, offset, count));
		}

		public ProtoBuf.IExtensible Deserialize(Stream stream)
		{
			var messageType = new MessageType();
			messageType.Deserialize(stream);
			return deserializeTable[messageType](stream);
		}

		#endregion

		#region Register
		/// <summary>注册可被解析的消息类型</summary>
		/// <typeparam name="T">可被解析的消息类型ID</typeparam>
		/// <param name="messageTypeID"><typeparamref name="T"/>对应的<see cref="ProtoBuf.IExtensible"/>类型</param>
		public void Register<T>(MessageType messageTypeID) where T : ProtoBuf.IExtensible
		{
			// 反序列化预编译
			ProtoBuf.Serializer.PrepareSerializer<T>();

			// 注册
			messageTypeTable[typeof(T)] = messageTypeID;
			//deserializeTable[messageType] = (stream) => ProtoBuf.Serializer.Deserialize<T>(stream);
			deserializeTable[messageTypeID] = (stream) => ProtoBuf.Serializer.DeserializeWithLengthPrefix<T>(stream, ProtoBuf.PrefixStyle.Base128);
		}

		/// <summary>注册可被解析的消息类型</summary>
		/// <param name="messageTypeID">可被解析的消息类型ID</param>
		/// <param name="messageType"><paramref name="messageTypeID"/>对应的<see cref="ProtoBuf.IExtensible"/>类型</param>
		/// <remarks>对泛型重载Register&lt;T&gt;的非泛型包装</remarks>
		public void Register(MessageType messageTypeID, Type messageType)
		{
			// Call Register<messageType>(messageTypeID) by refelect.
			this.GetType().GetRuntimeMethod("Register", typeof(MessageType))
				.MakeGenericMethod(messageType)
				.Invoke(this, new object[] { messageTypeID });
		}

		/// <summary>
		/// 解析消息号和消息类型对应表
		/// </summary>
		/// <returns></returns>
		private IEnumerable<KeyValuePair<MessageType, Type>> Parse()
		{
			var categoryIdType = typeof(Cmd.MSGTYPE.Cmd);
#if UNITY_METRO && !UNITY_EDITOR
			var assembly = this.GetType().GetTypeInfo().Assembly;
#else
			var assembly = Assembly.GetExecutingAssembly();
#endif
			var ret = new Dictionary<MessageType, Type>();
			foreach (var cName in Enum.GetNames(categoryIdType))
			{
				var cValue = Convert.ToByte(Enum.Parse(categoryIdType, cName));
				var typeIdType = assembly.GetType(categoryIdType.Namespace + "." + cName + ".MSGTYPE+Param");
				foreach (var tName in Enum.GetNames(typeIdType))
				{
					var tValue = Convert.ToByte(Enum.Parse(typeIdType, tName));
					var messageType = assembly.GetType(categoryIdType.Namespace + "." + cName + "." + tName);
					ret.Add(new MessageType() { Cmd = cValue, Param = tValue }, messageType);
				}
			}

			return ret;
		}
		#endregion

		public MessageSerializer()
		{
			foreach (var msg in Parse())
				Register(msg.Key, msg.Value);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (var pair in messageTypeTable)
			{
				sb.AppendLine(pair.Value + ":" + pair.Key);
			}
			return sb.ToString();
		}
	}
}
