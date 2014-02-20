#if UNITY_WP8
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

static class WP8Menu
{
	const string Temp = "WP8Temp";
	[MenuItem("WP8/Export", false)]
	public static void Help()
	{
		Debug.developerConsoleVisible = true;
		ClearLog();

		// 获取所有需要编译的场景列表
		var levels = (from s in EditorBuildSettings.scenes where s.enabled select s.path).ToArray();
		Debug.Log(string.Join("\n", levels));

		// 将 Assets/DLL 下的dll文件移动到临时目录，因这些dll不能兼容WP8下的生成
		Directory.CreateDirectory(Temp);
		try
		{
			ProtobufBegin();
			UniWebBegin();

			// 进行WP8项目生成
			{
				AssetDatabase.Refresh();
				BuildPipeline.BuildPlayer(levels, "WP8", BuildTarget.WP8Player, BuildOptions.None);
			}

			ProtobufEnd();
			UniWebEnd();

			Debug.Log("OK");
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
		finally
		{
			Directory.Delete(Temp, true);
			// 刷新资源
			AssetDatabase.Refresh();
		}
	}

	/// <summary>
	/// http://answers.unity3d.com/questions/10580/editor-script-how-to-clear-the-console-output-wind.html
	/// </summary>
	public static void ClearLog()
	{
		var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
		var type = assembly.GetType("UnityEditorInternal.LogEntries");
		var method = type.GetMethod("Clear");
		method.Invoke(new object(), null);
	}

	private static void ProtobufBegin()
	{
		// 默认的protobuf无法适应WP8编译
		File.Move("Assets/DLL/protobuf-net.dll", Temp + "/protobuf-net.dll");
		File.Move("Assets/DLL/protobuf-net.pdb", Temp + "/protobuf-net.pdb");
		File.Move("Assets/DLL/protobuf-net.xml", Temp + "/protobuf-net.xml");

		// 但cf20版本的可以通过WP8编译，借过来忽悠Unity
		File.Copy("3Party/protobuf-net r668/Full/cf20/protobuf-net.dll", "Assets/DLL/protobuf-net.dll", true);
		File.Copy("3Party/protobuf-net r668/Full/cf20/protobuf-net.pdb", "Assets/DLL/protobuf-net.pdb", true);
		File.Copy("3Party/protobuf-net r668/Full/cf20/protobuf-net.xml", "Assets/DLL/protobuf-net.xml", true);
	}

	private static void ProtobufEnd()
	{
		// 从临时文件夹恢复默认的protobuf文件
		File.Copy(Temp + "/protobuf-net.dll", "Assets/DLL/protobuf-net.dll", true);
		File.Copy(Temp + "/protobuf-net.pdb", "Assets/DLL/protobuf-net.pdb", true);
		File.Copy(Temp + "/protobuf-net.xml", "Assets/DLL/protobuf-net.xml", true);
	}

	private static void UniWebBegin()
	{
		Directory.Move("Assets/UniWeb", Temp + "/UniWeb");
	}

	private static void UniWebEnd()
	{
		Directory.Delete("Assets/UniWeb");
		Directory.Move(Temp + "/UniWeb", "Assets/UniWeb");
	}
}
#endif
