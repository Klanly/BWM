#if UNITY_WP8 || UNITY_WINRT
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

static class GXMenu
{
	const string Temp = "GXTemp";
#if UNITY_WP8
	[MenuItem("GX/Export WP8", false)]
#elif UNITY_WINRT
	[MenuItem("GX/Export Universal App", false)]
#endif
	public static void Help()
	{
		Debug.developerConsoleVisible = true;
		GX.Editor.LogEntries.Clear();

		// 获取所有需要编译的场景列表
		var levels = (from s in EditorBuildSettings.scenes where s.enabled select s.path).ToArray();
		Debug.Log(string.Join("\n", levels));

		// 将 Assets/DLL 下的dll文件移动到临时目录，因这些dll不能兼容WP8下的生成
		Directory.CreateDirectory(Temp);
		try
		{
			ProtobufBegin();
			WebSocket4NetBegin();
			JsonFxBegin();

			// 进行WP8项目生成
			{
#if UNITY_WP8
				AssetDatabase.Refresh();
				BuildPipeline.BuildPlayer(levels, "WP8", BuildTarget.WP8Player, BuildOptions.None);
#elif UNITY_WINRT
				/*
				var metroSDK = UnityEditor.EditorUserBuildSettings.metroSDK;
				try
				{
					{
						PlayerSettings.productName = "BWGame-8.0";
						UnityEditor.EditorUserBuildSettings.metroSDK = MetroSDK.SDK80;
						AssetDatabase.Refresh();
						BuildPipeline.BuildPlayer(levels, "WinRT", BuildTarget.MetroPlayer, BuildOptions.None);
					}

					{
						PlayerSettings.productName = "BWGame-8.1";
						UnityEditor.EditorUserBuildSettings.metroSDK = MetroSDK.SDK81;
						AssetDatabase.Refresh();
						BuildPipeline.BuildPlayer(levels, "WinRT", BuildTarget.MetroPlayer, BuildOptions.None);
					}
				}
				finally
				{
					PlayerSettings.productName = "BWGame";
					UnityEditor.EditorUserBuildSettings.metroSDK = metroSDK;
				}
				*/
				{
					PlayerSettings.productName = "BWGame";
					UnityEditor.EditorUserBuildSettings.metroSDK = MetroSDK.UniversalSDK81;

					// 避免 UNITY_EDITOR* 在导出时没有被取消定义
					PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Metro, string.Join(";",
						UnityEditor.EditorUserBuildSettings.activeScriptCompilationDefines
						.Where(i => i.StartsWith("UNITY_EDITOR") == false)
						.ToArray()));
					Debug.Log("DefineSymbols: " + PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Metro));

					AssetDatabase.Refresh();
					BuildPipeline.BuildPlayer(levels, "WS", BuildTarget.MetroPlayer, BuildOptions.None);
				}
#endif
			}

			ProtobufEnd();
			WebSocket4NetEnd();
			JsonFxEnd();

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

	private static void WebSocket4NetBegin()
	{
		File.Move("Assets/DLL/WebSocket4Net.dll", Temp + "/WebSocket4Net.dll");
	}

	private static void WebSocket4NetEnd()
	{
		File.Copy(Temp + "/WebSocket4Net.dll", "Assets/DLL/WebSocket4Net.dll", true);
	}

	private static void JsonFxBegin()
	{
		File.Move("Assets/DLL/JsonFx.Json.dll", Temp + "/JsonFx.Json.dll");
	}

	private static void JsonFxEnd()
	{
		File.Copy(Temp + "/JsonFx.Json.dll", "Assets/DLL/JsonFx.Json.dll", true);
	}
}
#endif
