using UnityEngine;
using System.Collections;
using System.Linq;

public class ControlBar : MonoBehaviour
{
	public UIButton roleInfoButton;
	public UIButton skillInfoButton;
	public UIButton[] uiSkillFireThumbs;

	void Start()
	{
		// 切换RoleInfo界面的显隐
		UIEventListener.Get(roleInfoButton.gameObject).onClick = go =>
		{
			var target = BattleScene.Instance.Gui<RoleInfo>();
			target.gameObject.SetActive(!target.gameObject.activeSelf);
		};

		// 切换SkillInfo界面的显隐
		UIEventListener.Get(skillInfoButton.gameObject).onClick = go =>
		{
			var target = BattleScene.Instance.Gui<SkillInfo>();
			target.gameObject.SetActive(!target.gameObject.activeSelf);
		};

		// 技能施法按钮点击
		for (var i = 0; i < uiSkillFireThumbs.Length; i++)
		{
			var index = i;
			UIEventListener.Get(uiSkillFireThumbs[index].gameObject).onClick = go => 
				SkillManager.Instance.FireSkill(Config.UserData.Instance.skillbar[index]);
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
}
