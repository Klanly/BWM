using UnityEngine;
using System.Collections;

public class GXRoleHead : MonoBehaviour
{
	public UILabel myName;
	public UILabel myLevel;

	public UILabel otherName;
	public UILabel otherLevel;

	void Start()
	{
		if (MainCharacter.ServerInfo.data == null)
			return;
		myName.text = MainCharacter.ServerInfo.data.charname;
	}
}
