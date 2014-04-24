using UnityEngine;
using System.Collections;

public class RoleInfo : MonoBehaviour
{
	/// <summary>背包</summary>
	public UIButton uiRoleInfoPackage;
	/// <summary>属性</summary>
	public UIButton uiRoleInfoValue;

	/// <summary>技能</summary>
	public UIButton uiSkillInfo;

	void Start()
	{
		UIEventListener.Get(uiRoleInfoPackage.gameObject).onClick = go => ShowRightPanel<RoleInfoPackage>();
		UIEventListener.Get(uiRoleInfoValue.gameObject).onClick = go => ShowRightPanel<RoleInfoValue>();

		UIEventListener.Get(uiSkillInfo.gameObject).onClick = go =>
		{
			var target = BattleScene.Instance.Gui<SkillInfo>();
			target.gameObject.SetActive(!target.gameObject.activeSelf);
		};

		ShowRightPanel<RoleInfoPackage>();
	}

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}

	private T ShowRightPanel<T>() where T : MonoBehaviour
	{
		T show = null;
		foreach(var t in new System.Type[]{typeof(RoleInfoPackage), typeof(RoleInfoValue)})
		{
			if (t == typeof(T))
			{
				show = BattleScene.Instance.Gui(t) as T;
				show.gameObject.SetActive(true);
				NGUITools.BringForward(show.gameObject);
			}
			else
			{
				var hide = BattleScene.Instance.Gui(t);
				hide.gameObject.SetActive(false);
			}
		}

		NGUITools.BringForward(this.GetComponent<Closeable>().closeButton.gameObject);
		return show;
	}
}
