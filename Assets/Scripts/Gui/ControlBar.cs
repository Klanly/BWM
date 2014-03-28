using UnityEngine;
using System.Collections;
using System.Linq;

public class ControlBar : MonoBehaviour
{
	public UIButton roleInfoButton;
	public UIButton skillInfoButton;

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
	}
}
