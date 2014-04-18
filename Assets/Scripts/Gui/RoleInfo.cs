using UnityEngine;
using System.Collections;

public class RoleInfo : MonoBehaviour
{
	public UIButton uiSkillInfo;

	void Start()
	{
		UIEventListener.Get(uiSkillInfo.gameObject).onClick = go =>
		{
			var target = BattleScene.Instance.Gui<SkillInfo>();
			target.gameObject.SetActive(!target.gameObject.activeSelf);
		};
	}
	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}
}
