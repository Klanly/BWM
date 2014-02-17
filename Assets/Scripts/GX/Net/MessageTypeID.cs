using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GX.Net
{
	/// <summary>
	/// 用数字表示的<see cref="ProtoBuf.IExtensible"/>消息类型
	/// </summary>
	public struct MessageTypeID : IComparable<MessageTypeID>
	{
		public byte CmdID { get; set; }
		public byte ParamID { get; set; }

		#region IComparable<MessageType> 成员

		public int CompareTo(MessageTypeID other)
		{
			if (this.CmdID > other.CmdID)
				return 1;
			else if (this.CmdID < other.CmdID)
				return -1;

			if (this.ParamID > other.ParamID)
				return 1;
			else if (this.ParamID < other.ParamID)
				return -1;

			return 0;
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{{Cmd={0},Param={1}}}", CmdID, ParamID);
		}

		public override int GetHashCode()
		{
			return ((int)CmdID << 8) | ParamID;
		}

		public static IEnumerable<KeyValuePair<MessageTypeID, Type>> Parse()
		{
			var categoryIdType = typeof(Cmd.MSGTYPE.Cmd);
			var assembly = Assembly.GetExecutingAssembly();

			var ret = new SortedList<MessageTypeID, Type>();
			foreach (var cName in Enum.GetNames(categoryIdType))
			{
				var cValue = Convert.ToByte(Enum.Parse(categoryIdType, cName));
				var typeIdType = assembly.GetType(categoryIdType.Namespace + "." + cName + ".MSGTYPE+Param", true);
				foreach (var tName in Enum.GetNames(typeIdType))
				{
					var tValue = Convert.ToByte(Enum.Parse(typeIdType, tName));
					var messageType = assembly.GetType(categoryIdType.Namespace + "." + cName + "." + tName, true);
					ret.Add(new MessageTypeID() { CmdID = cValue, ParamID = tValue }, messageType);
				}
			}

			return ret;
		}

		public void Serialize(System.IO.Stream stream)
		{
			stream.WriteByte(this.CmdID);
			stream.WriteByte(this.ParamID);
		}

		public void Deserialize(System.IO.Stream stream)
		{
			var cmd = stream.ReadByte();
			if (cmd == -1) throw new System.IO.EndOfStreamException();
			this.CmdID = (byte)cmd;

			var param = stream.ReadByte();
			if (param == -1) throw new System.IO.EndOfStreamException();
			this.ParamID = (byte)param;
		}
	}
}
