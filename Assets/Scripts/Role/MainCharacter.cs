using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using GX;
using MainCharacterInfo = Cmd.FirstMainUserDataAndPosMapUserCmd_S;

public class MainCharacter : Singleton<MainCharacter>
{
	public static MainCharacterInfo ServerInfo { get; private set; }

	static MainCharacter()
	{
		ServerInfo = new MainCharacterInfo(); // 避免不必要的空指针判断
	}

	/// <summary>
	/// 更新主角信息
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	static IEnumerator Execute(MainCharacterInfo cmd)
	{
		ServerInfo = cmd;
		if (Application.loadedLevelName != "BattleScene")
		{
			Application.LoadLevel("BattleScene");
			yield return null;
		}
	}
}
