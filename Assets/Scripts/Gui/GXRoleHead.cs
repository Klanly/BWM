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
		myName.text = MainRole.ServerInfo.charname;
		myLevel.text = MainRole.ServerInfo.level.ToString();
	}
}
