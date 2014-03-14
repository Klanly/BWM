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
		if (MainRole.ServerInfo == null)
			return;
		myName.text = MainRole.ServerInfo.charname;
	}
}
