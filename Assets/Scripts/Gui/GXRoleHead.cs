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
		var my = myName.transform.parent; // 主角自我信息所在节点
		my.Find("head1").gameObject.SetActive(MainRole.ServerInfo.sexman);
		my.Find("head2").gameObject.SetActive(!MainRole.ServerInfo.sexman);

		myName.text = MainRole.ServerInfo.charname;
		myLevel.text = MainRole.Instance.level.ToString();
	}
}
