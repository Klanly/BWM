using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class RoleHead : MonoBehaviour
{
	public UILabel myName;
	public UILabel myLevel;
	public UISlider myHp;
	public UILabel	myHpText;
	public UISprite myHead;

	IEnumerator Start()
	{
		while (MainRole.Instance == null)
			yield return new WaitForEndOfFrame();
		MainRole.Instance.PropertyChanged += OnMainRolePropertyChanged;
		OnMainRolePropertyChanged(this, null);
	}

	void OnDestroy()
	{
		if (MainRole.Instance != null)
		{
			MainRole.Instance.PropertyChanged -= OnMainRolePropertyChanged;
		}
	}

	void OnMainRolePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		myName.text = MainRole.ServerInfo.userdata.charname;
		myLevel.text = MainRole.ServerInfo.level.ToString();
		myHead.spriteName = MainRole.ServerInfo.userdata.GetRoleHeadSprite();
		myHp.value = MainRole.Instance.Role.ServerInfo.hp / (float)MainRole.Instance.Role.ServerInfo.maxhp;
		myHpText.text = MainRole.Instance.Role.ServerInfo.hp + "/" + MainRole.Instance.Role.ServerInfo.maxhp;
	}
}
