using UnityEngine;
using System.Collections;
using System.Linq;

public class ControlBar : MonoBehaviour
{
	public UIButton roleInfoButton;
	public UIButton[] uiSkillFireThumbs;
	public UILabel[] uiSkillCD;
	public UIButton uiSkillBasic;
	public UILabel uiSkillBasicCD;
	public GXJoystick joystick;

	void Start()
	{
		// 切换RoleInfo界面的显隐
		UIEventListener.Get(roleInfoButton.gameObject).onClick = go =>
		{
			var target = BattleScene.Instance.Gui<RoleInfo>();
			target.gameObject.SetActive(!target.gameObject.activeSelf);
		};

		// 技能施法按钮点击
		UIEventListener.Get(uiSkillBasic.gameObject).onClick = go =>
			SkillManager.Fire(SkillManager.Instance.BasicSkill);
		for (var i = 0; i < uiSkillFireThumbs.Length; i++)
		{
			var index = i;
			UIEventListener.Get(uiSkillFireThumbs[index].gameObject).onClick = go =>
				SkillManager.Fire(Config.UserData.Instance.skillbar[index]);
		}

		Config.UserData.Instance.PropertyChanged += OnConfigUserDataChanged;
		OnConfigUserDataChanged(this, null);
	}

	void OnDestroy()
	{
		Config.UserData.Instance.PropertyChanged -= OnConfigUserDataChanged;
	}


	void OnConfigUserDataChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e != null && e.PropertyName != "skillbar")
			return;
		SkillInfo.PresentFireThumbs(this.uiSkillFireThumbs);
	}

	void Update()
	{
		if (joystick) 
		{
			float horizontal = Input.GetAxis ("Horizontal");
			float vertical = Input.GetAxis ("Vertical");
			if (horizontal == 0 && vertical == 0) 
			{
				horizontal = joystick.position.x;
				vertical = joystick.position.y;
			}
			if(MainRole.Instance)
			{
				MainRole.Instance.controlMove.MoveByJoystick(horizontal, vertical, joystick.pressed);
			}
		}

		// 更新CD
		UpdateCD(SkillManager.Instance.BasicSkill, this.uiSkillBasicCD, this.uiSkillBasic);
		for (var i = 0; i < uiSkillFireThumbs.Length; i++)
			UpdateCD(Config.UserData.Instance.skillbar[i], uiSkillCD[i], uiSkillFireThumbs[i]);
	}

	void UpdateCD(uint skill, UILabel label, UIButton button)
	{
		var cd = SkillManager.Instance.CoolDown(skill);
		if (cd < 0)
		{
			label.gameObject.SetActive(false);
			button.isEnabled = true;
		}
		else
		{
			label.gameObject.SetActive(true);
			label.text = cd.ToString("F1") + "s";
			button.isEnabled = false;
		}
	}
}
