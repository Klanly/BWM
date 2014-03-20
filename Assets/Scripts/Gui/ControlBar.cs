using UnityEngine;
using System.Collections;

public class ControlBar : MonoBehaviour
{
	public UIButton roleInfoButton;

	void Start()
	{
		// 切换RoleInfo界面的显隐
		UIEventListener.Get(roleInfoButton.gameObject).onClick = go =>
		{
			var target = BattleScene.Instance.Gui<RoleInfo>();
			NGUITools.SetActive(target.gameObject, !target.gameObject.activeSelf);
			if (target.gameObject.activeSelf)
				NGUITools.BringForward(this.gameObject);
		};
	}
}
