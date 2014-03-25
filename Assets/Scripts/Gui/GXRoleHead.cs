using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class GXRoleHead : MonoBehaviour
{
	public UILabel myName;
	public UILabel myLevel;
	public UISlider myHp;
	public UISprite myHead;

	private ISelectTarget m_target;
	public ISelectTarget Target
	{
		get { return m_target; }
		set
		{
			this.m_target = value;
			OnSelectTargetChanged();
		}
	}
	public UILabel otherName;
	public UISprite otherHead;

	IEnumerator Start()
	{
		Target = null;
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
		myName.text = MainRole.ServerInfo.charname;
		myLevel.text = MainRole.Instance.level.ToString();
		myHead.spriteName = MainRole.ServerInfo.GetRoleHeadSprite();
		myHp.value = MainRole.Instance.hp / (float)MainRole.Instance.maxhp;
	}

	void OnSelectTargetChanged()
	{
		if (Target == null)
		{
			this.otherName.parent.gameObject.SetActive(false);
			return;
		}
		this.otherName.parent.gameObject.SetActive(true);
		this.otherName.text = Target.Name;
		this.otherHead.spriteName = Target.RoleHeadSprite;
	}
}
