using UnityEngine;
using System.Collections;
using System.Linq;

public class SkillInfo : MonoBehaviour
{
	public UIButton uiClose;
	public SkillInfoItem[] uiItems;
	public UISprite[] uiSkillButtons;

	void Start()
	{
		UIEventListener.Get(uiClose.gameObject).onClick = go => this.gameObject.SetActive(false);
		Present();
	}

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}

	void Present()
	{
		foreach (var i in uiItems.Zip(SkillManager.Instance.OrderBy(i => i.Key)))
			i.Item1.Skill = i.Item2;
	}
}
