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
		public static readonly MessageType Empty = new MessageType();
			 
		public uint Cmd { get; set; }
		public uint Param { get; set; }

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
	}
}
