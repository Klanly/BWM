using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace GX.Net
{
	/// <summary>
	/// 标志是一个可以响应消息的方法
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class ExecuteAttribute : Attribute
	{
		#region 具有ExecuteAttribute特性函数的自动提取
		/// <summary>
		/// 得到类中所有具有<see cref="ExecuteAttribute"/>特性的静态方法
		/// </summary>
		public static IEnumerable<MethodInfo> GetStaticExecuteMethod(Type type)
		{
			return GetExecuteMethod(type, BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public
				| BindingFlags.Static);
		}

		/// <summary>
		/// 得到所给汇编中所有具有<see cref="ExecuteAttribute"/>特性的静态方法
		/// </summary>
		public static IEnumerable<MethodInfo> GetStaticExecuteMethod(Assembly assembly)
		{
			return (
				from module in assembly.GetModules()
				from type in module.GetTypes()
				select GetStaticExecuteMethod(type))
				.SelectMany(s => s);
		}

		/// <summary>
		/// 得到对象中所有具有<see cref="ExecuteAttribute"/>特性的方法
		/// </summary>
		public static IEnumerable<MethodInfo> GetInstanceExecuteMethod(Type targetType)
		{
			return GetExecuteMethod(targetType, BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public
				| BindingFlags.Instance);
		}

		private static IEnumerable<MethodInfo> GetExecuteMethod(Type targetType, BindingFlags flags)
		{
			return
				from method in targetType.GetMethods(flags)
				let tag = Attribute.GetCustomAttribute(method, typeof(ExecuteAttribute)) as ExecuteAttribute
				where tag != null
				select method;
		}
		#endregion
	}
}