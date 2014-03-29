using UnityEngine;
using System.Collections;
using System.Linq;

public class SkillInfo : MonoBehaviour
{
	public UIButton uiClose;
	public UISprite[] uiSkillButtons;
	private SkillInfoItem[] items;

	void Start()
	{
		UIEventListener.Get(uiClose.gameObject).onClick = go => this.gameObject.SetActive(false);

		// 技能格子初始化
		var grid = this.transform.FindChild("SkillBar");
		items = new SkillInfoItem[grid.childCount];
		for (var i = 0; i < grid.childCount; i++)
		{
			var view = grid.GetChild(i).GetComponent<SkillInfoItem>();
			items[i] = view;
			view.gameObject.name = i.ToString("D2");

			// 格子点击
			UIEventListener.Get(grid.GetChild(i).gameObject).onClick = go => OnSkillItemClicked(view);
		}

		SkillManager.Instance.SkillChanged += Present;
		Present(SkillManager.Instance);
	}

	void OnDestroy()
	{
		SkillManager.Instance.SkillChanged -= Present;
	}


	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
		Present(SkillManager.Instance);
	}

	void Present(SkillManager manager)
	{
		if (this.gameObject.activeSelf == false || items == null)
			return;
		foreach (var i in items.Zip(manager.OrderBy(i => i.Key)))
			i.Item1.Skill = i.Item2;
	}

	private void OnSkillItemClicked(SkillInfoItem view)
	{
	}
}
