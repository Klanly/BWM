using UnityEngine;
using System.Collections;
using System.Linq;

public class ControlBar : MonoBehaviour
{
	public UIButton roleInfoButton;

	void Start()
	{
		// 切换RoleInfo界面的显隐
		UIEventListener.Get(roleInfoButton.gameObject).onClick = go =>
		{
			var target = BattleScene.Instance.Gui<RoleInfo>();
			BattleScene.Instance.Gui<RoleInfo>().gameObject.SetActive(!target.gameObject.activeSelf);
		};
	}
}
