using UnityEngine;
using System.Collections;

public class SkillInfo : MonoBehaviour
{
	public UIButton uiClose;
	public SkillInfoItem[] uiItems;
	public UISprite[] uiSkillButtons;

	void Start()
	{
		UIEventListener.Get(uiClose.gameObject).onClick = go => this.gameObject.SetActive(false);
	}
	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}
}
