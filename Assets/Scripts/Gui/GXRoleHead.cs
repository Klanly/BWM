using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class GXRoleHead : MonoBehaviour
{
	public UILabel myName;
	public UILabel myLevel;
	public UISlider myHp;

	public UILabel otherName;
	public UILabel otherLevel;

	void Start()
	{
		MainRole.Instance.PropertyChanged += OnMainRolePropertyChanged;
		OnMainRolePropertyChanged(this, null);
	}

	void OnDestroy()
	{
		if (MainRole.Instance != null)
			MainRole.Instance.PropertyChanged -= OnMainRolePropertyChanged;
	}

	void OnMainRolePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		var my = myName.transform.parent; // 主角自我信息所在节点
		my.Find("head1").gameObject.SetActive(MainRole.ServerInfo.sexman);
		my.Find("head2").gameObject.SetActive(!MainRole.ServerInfo.sexman);

		myName.text = MainRole.ServerInfo.charname;
		myLevel.text = MainRole.Instance.level.ToString();

		myHp.value = MainRole.Instance.hp / (float)MainRole.Instance.maxhp;
	}
}
