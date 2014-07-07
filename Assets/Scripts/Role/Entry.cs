using UnityEngine;
using System.Collections;

public class Entry : MonoBehaviour
{

	public Cmd.SceneEntryUid uid = new Cmd.SceneEntryUid();

	// Use this for initialization
	void Start()
	{
		if (GetComponent<Npc>() != null)
		{
			uid.entrytype = Cmd.SceneEntryType.SceneEntryType_Npc;
			uid.entryid = GetComponent<Npc>().ServerInfo.tempid;
		}
		else if (GetComponent<Role>() != null)
		{
			uid.entrytype = Cmd.SceneEntryType.SceneEntryType_Player;
			uid.entryid = GetComponent<Role>().ServerInfo.charid;
		}
		else if (GetComponent<SceneItem>() != null)
		{
			uid.entrytype = Cmd.SceneEntryType.SceneEntryType_Item;
			uid.entryid = GetComponent<SceneItem>().ServerInfo.thisid;
		}
	}

	public bool IsMainRole()
	{
		if (uid.entrytype == Cmd.SceneEntryType.SceneEntryType_Player && uid.entryid == MainRole.Instance.Role.ServerInfo.charid)
			return true;

		return false;
	}

	public bool IsPlayer()
	{
		if (uid.entrytype == Cmd.SceneEntryType.SceneEntryType_Player)
			return true;

		return false;
	}

	public bool IsNpc()
	{
		if (uid.entrytype == Cmd.SceneEntryType.SceneEntryType_Npc)
			return true;

		return false;
	}

	bool IsItem()
	{
		if (uid.entrytype == Cmd.SceneEntryType.SceneEntryType_Item)
			return true;

		return false;
	}
}
