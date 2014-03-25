using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;

public class SelectTarget : MonoBehaviour
{
	public void Show<T>(object data) where T : MonoBehaviour, ISelectTarget
	{
		foreach (var c in this.transform.GetAllChildren())
		{
			var show = c.gameObject.GetComponent<T>();
			c.gameObject.SetActive(data != null && show != null);
			//if (c.gameObject.active)
			//	show.Present(data);
		}
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// 场景点选
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	static void Execute(SelectSceneEntryScriptUserCmd_CS cmd)
	{
		switch (cmd.entrytype)
		{
			case SceneEntryType.SceneEntryType_Npc:
				//Npc.SceneSelect(cmd.entryid);
				break;
			case SceneEntryType.SceneEntryType_Player:
				//Role.SceneSelect(cmd.entryid);
				break;
			default:
				break;
		}
	}

}
