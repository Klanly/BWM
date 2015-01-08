using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEngine;
using GX.Net;
using GX;

public class MessageSerializer : GX.Net.MessageSerializer
{
	private readonly Dictionary<MessageType, Func<Stream, ProtoBuf.IExtensible>> deserializeTable = new Dictionary<MessageType, Func<Stream, ProtoBuf.IExtensible>>();

	public MessageSerializer()
	{
		foreach (var msg in Parse(typeof(Cmd.Command)).Concat(Parse(typeof(Pmd.PlatCommand))))
			Register(msg.Key, msg.Value);
	}

	public override void Register<T>(MessageType messageTypeID)
	{
		base.Register<T>(messageTypeID);
		deserializeTable[messageTypeID] = (stream) => ProtoBuf.Serializer.DeserializeWithLengthPrefix<T>(stream, ProtoBuf.PrefixStyle.Base128);
	}

	public override void SerializeTo(Stream stream, ProtoBuf.IExtensible message)
	{
		var messageType = this[message.GetType()];
		var package = new Pmd.ForwardNullUserPmd_CS()
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

	public override ProtoBuf.IExtensible Deserialize(Stream stream)
	{
		var package = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Pmd.ForwardNullUserPmd_CS>(stream, ProtoBuf.PrefixStyle.Base128);
		var messageType = new MessageType() { Cmd = package.byCmd, Param = package.byParam };
		using (var buf = new MemoryStream(package.data))
		{
			try
			{
				return deserializeTable[messageType](buf);
			}
			catch (Exception ex)
			{
				var type = (from i in this where i.Value == messageType select i.Key).FirstOrDefault();
				UnityEngine.Debug.LogWarning(string.Format("Can't deserialize message type {0}{1}, {2}",
					type == null ? string.Empty : "<color=orange>" + type.FullName + "</color> ",
					"<color=yellow>" + messageType + "</color>",
					ex.Message));
				return null;
			}
		}
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
}
