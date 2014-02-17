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
	public struct MessageType : IComparable<MessageType>
	{
		public byte Cmd { get; set; }
		public byte Param { get; set; }

		#region IComparable<MessageType> 成员

		public int CompareTo(MessageType other)
		{
			if (this.Cmd > other.Cmd)
				return 1;
			else if (this.Cmd < other.Cmd)
				return -1;

			if (this.Param > other.Param)
				return 1;
			else if (this.Param < other.Param)
				return -1;

			return 0;
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{{Cmd={0},Param={1}}}", Cmd, Param);
		}

		public override int GetHashCode()
		{
			return ((int)Cmd << 8) | Param;
		}

		public void Serialize(System.IO.Stream stream)
		{
			stream.WriteByte(this.Cmd);
			stream.WriteByte(this.Param);
		}

		public void Deserialize(System.IO.Stream stream)
		{
			var cmd = stream.ReadByte();
			if (cmd == -1) throw new System.IO.EndOfStreamException();
			this.Cmd = (byte)cmd;

			var param = stream.ReadByte();
			if (param == -1) throw new System.IO.EndOfStreamException();
			this.Param = (byte)param;
		}
	}
}
