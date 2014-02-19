#if UNITY_WP8
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

static class WP8Menu
{
	const string Temp = "WP8Temp";
	[MenuItem("WP8/Export", false)]
	public static void Help()
	{
		// 获取所有需要编译的场景列表
		var levels = (from s in EditorBuildSettings.scenes where s.enabled select s.path).ToArray();
		Debug.Log(string.Join("\n", levels));

		// 将 Assets/DLL 下的dll文件移动到临时目录，因这些dll不能兼容WP8下的生成
		Directory.CreateDirectory(Temp);
		try
		{
			// 默认的protobuf无法适应WP8编译
			File.Move("Assets/DLL/protobuf-net.dll", Temp + "/protobuf-net.dll");
			File.Move("Assets/DLL/protobuf-net.pdb", Temp + "/protobuf-net.pdb");
			File.Move("Assets/DLL/protobuf-net.xml", Temp + "/protobuf-net.xml");

			// 但cf20版本的可以通过WP8编译，借过来忽悠Unity
			File.Copy("3Party/protobuf-net r668/Full/cf20/protobuf-net.dll", "Assets/DLL/protobuf-net.dll", true);
			File.Copy("3Party/protobuf-net r668/Full/cf20/protobuf-net.pdb", "Assets/DLL/protobuf-net.pdb", true);
			File.Copy("3Party/protobuf-net r668/Full/cf20/protobuf-net.xml", "Assets/DLL/protobuf-net.xml", true);

			// 进行WP8项目生成
			{
				AssetDatabase.Refresh();
				BuildPipeline.BuildPlayer(levels, "WP8", BuildTarget.WP8Player, BuildOptions.None);
			}

			// 生成的WP8项目采用wp8版本的protobuf
			File.Copy("3Party/protobuf-net r668/Full/wp8/protobuf-net.dll", "WP8/ShenZuo/protobuf-net.dll", true);
			File.Copy("3Party/protobuf-net r668/Full/wp8/protobuf-net.pdb", "WP8/ShenZuo/protobuf-net.pdb", true);
			File.Copy("3Party/protobuf-net r668/Full/wp8/protobuf-net.xml", "WP8/ShenZuo/protobuf-net.xml", true);

			// 从临时文件夹恢复默认的protobuf文件
			File.Copy(Temp + "/protobuf-net.dll", "Assets/DLL/protobuf-net.dll", true);
			File.Copy(Temp + "/protobuf-net.pdb", "Assets/DLL/protobuf-net.pdb", true);
			File.Copy(Temp + "/protobuf-net.xml", "Assets/DLL/protobuf-net.xml", true);
		}
		finally
		{
			Directory.Delete(Temp, true);
		}

		// 刷新资源
		AssetDatabase.Refresh();

		Debug.Log("OK");
	}
}
#endif
