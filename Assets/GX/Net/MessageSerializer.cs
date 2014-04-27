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

		public MessageType this[Type type]
		{
			get 
			{
				MessageType mt;
				return messageTypeTable.TryGetValue(type, out mt) ? mt : MessageType.Empty;
			}
		}

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
			var package = new Cmd.ForwardNullUserCmd_CS()
			{
				byCmd = messageType.Cmd,
				byParam = messageType.Param,
				time = DateTime.Now.ToUnixTime(),
			};

			using (var buf = new MemoryStream())
			{
				ProtoBuf.Serializer.NonGeneric.SerializeWithLengthPrefix(buf, message, ProtoBuf.PrefixStyle.Base128, 0);
				package.data = buf.ToArray();
			}

			ProtoBuf.Serializer.NonGeneric.SerializeWithLengthPrefix(stream, package, ProtoBuf.PrefixStyle.Base128, 0);
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
			var package = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Cmd.ForwardNullUserCmd_CS>(stream, ProtoBuf.PrefixStyle.Base128);
			var messageType = new MessageType() { Cmd = package.byCmd, Param = package.byParam };
			using (var buf = new MemoryStream(package.data))
			{
				try
				{
					return deserializeTable[messageType](buf);
				}
				catch (Exception ex)
				{
					var type = (from i in messageTypeTable where i.Value == messageType select i.Key).FirstOrDefault();
					UnityEngine.Debug.LogWarning(string.Format("Can't deserialize message type {0}{1}, {2}",
						type == null ? string.Empty : "<color=orange>" + type.FullName + "</color> ",
						"<color=yellow>" + messageType + "</color>", 
						ex.Message));
					return null;
				}
			}
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
		private static IEnumerable<KeyValuePair<MessageType, Type>> Parse(Type categoryIdType)
		{
#if UNITY_METRO && !UNITY_EDITOR
			var assembly = this.GetType().GetTypeInfo().Assembly;
#else
			var assembly = Assembly.GetExecutingAssembly();
#endif
			var ret = new Dictionary<MessageType, Type>();
			foreach (var cName in Enum.GetNames(categoryIdType))
			{
				var cValue = Convert.ToByte(Enum.Parse(categoryIdType, cName));
				var name1 = categoryIdType.Namespace + "." + cName + "+Param";
				var typeIdType = assembly.GetType(name1);
				if (typeIdType == null)
				{
					UnityEngine.Debug.LogWarning("Can't find type by name: " + name1);
					continue;
				}
				foreach (var tName in Enum.GetNames(typeIdType))
				{
					var tValue = Convert.ToByte(Enum.Parse(typeIdType, tName));
					var name2 = categoryIdType.Namespace + "." + tName;
					var messageType = assembly.GetType(name2);
					if (messageType == null)
					{
						UnityEngine.Debug.LogWarning("Can't find type by name: " + name2);
						continue;
					}
					ret.Add(new MessageType() { Cmd = cValue, Param = tValue }, messageType);
				}
			}
			return ret;
		}
		#endregion

		public MessageSerializer()
		{
			foreach (var msg in Parse(typeof(Cmd.Command)).Concat(Parse(typeof(Pmd.PlatCommand))))
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
