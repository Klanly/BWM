using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections;

namespace GX.Net
{
	/// <summary>
	/// 消息注册分发器
	/// </summary>
	public class MessageDispatcher
	{
		#region class MessageInvoker
		private abstract class MessageInvoker
		{
			class Invoker<TMessage> : MessageInvoker
				where TMessage : class, ProtoBuf.IExtensible
			{
				private Func<TMessage, IEnumerator> action;

				public override object Target { get { return action.Target; } }
				public override Type MessageType { get { return typeof(TMessage); } }

				public Invoker(Func<TMessage, IEnumerator> action)
				{
					this.action = action;
				}

				public Invoker(MethodInfo methodInfo, object target)
				{
					if (methodInfo.ReturnType == typeof(void))
					{
						this.action = new Func<TMessage, IEnumerator>(m =>
						{
							var act  = (Action<TMessage>)Delegate.CreateDelegate(typeof(Action<TMessage>), target, methodInfo, true);
							act(m);
							return null;
						});
					}
					else
					{
						this.action = (Func<TMessage, IEnumerator>)Delegate.CreateDelegate(typeof(Func<TMessage, IEnumerator>), target, methodInfo, true);
					}
				}

				public override IEnumerator Invoke(ProtoBuf.IExtensible message)
				{
					return this.action(message as TMessage);
				}

				public override string ToString()
				{
					return action.Method.ToString();
				}
			}

			public abstract object Target { get; }
			public abstract Type MessageType { get; }
			public abstract IEnumerator Invoke(ProtoBuf.IExtensible message);

			public static MessageInvoker Create(MethodInfo method, object target)
			{
				return Activator.CreateInstance(
					typeof(MessageInvoker.Invoker<>).MakeGenericType(GetMessageType(method)),
					method, target) as MessageInvoker;
			}

			/// <summary>
			/// 根据消息响应函数，得到其可以处理的消息类型
			/// </summary>
			private static Type GetMessageType(MethodInfo method)
			{
				var parameters = method.GetParameters();
				Debug.Assert(parameters.Count() == 1);
				Debug.Assert(parameters[0].ParameterType.GetInterface(typeof(ProtoBuf.IExtensible).FullName) != null);
				return method.GetParameters()[0].ParameterType;
			}
		} 
		#endregion

		private readonly Dictionary<Type, MessageInvoker> items = new Dictionary<Type,MessageInvoker>();

		/// <summary>
		/// 将消息发送到对应的接收者
		/// </summary>
		/// <param name="message"></param>
		/// <param name="result">消息接收者处理消息后的返回值</param>
		/// <returns>是否有对应的消息接收者并进行了分发</returns>
		public bool Dispatch(ProtoBuf.IExtensible message, out IEnumerator result)
		{
			MessageInvoker invoker;
			if (items.TryGetValue(message.GetType(), out invoker))
			{
				result = invoker.Invoke(message);
				return true;
			}
			else
			{
				result = null;
				return false;
			}
		}

		#region 消息响应注册

		/// <summary>
		/// 静态消息响应函数
		/// </summary>
		public void StaticRegister()
		{
			foreach(var method in ExecuteAttribute.GetStaticExecuteMethod(Assembly.GetExecutingAssembly()))
			{
				var invoker = MessageInvoker.Create(method, null);
				items.Add(invoker.MessageType, invoker);
			}
		}

		/// <summary>
		/// 注册对象的消息响应。
		/// 请及时调用UnRegister以取消注册，否则<paramref name="target"/>对象将不会被GC！
		/// </summary>
		public void Register(object target)
		{
			foreach(var method in ExecuteAttribute.GetInstanceExecuteMethod(target.GetType()))
			{
				var invoker = MessageInvoker.Create(method, target);
				items.Add(invoker.MessageType, invoker);
			}
		}

		/// <summary>
		/// 取消对象的消息响应注册
		/// </summary>
		/// <param name="target"></param>
		public void UnRegister(object target)
		{
			var keys = (from pair in items where pair.Value.Target == target select pair.Key).ToList();
			keys.ForEach(k => items.Remove(k));
		}

		#endregion

		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (var invoker in items.Values)
				sb.AppendLine(invoker.ToString());
			return sb.ToString();
		}
	}
}
