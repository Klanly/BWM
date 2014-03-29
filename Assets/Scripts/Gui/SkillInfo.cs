using UnityEngine;
using System.Collections;
using System.Linq;

public class SkillInfo : MonoBehaviour
{
	public UIButton uiClose;
	public UIButton[] uiSkillFireThumbs;
	public UIButton uiSkillFireOK;
	private SkillInfoItem[] items;
	private SkillInfoItem selected;

	public UILabel infoName;
	public UILabel infoRequire;
	public UILabel infoCD;
	public UILabel infoDesc;

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
			UIEventListener.Get(view.uiIcon.gameObject).onClick = go => PresentInfo(view);
		}

		// 技能释放按钮
		if(Config.UserData.Instance.skillbar.Count < uiSkillFireThumbs.Length)
			Config.UserData.Instance.skillbar.AddRange(Enumerable.Repeat(0u, uiSkillFireThumbs.Length - Config.UserData.Instance.skillbar.Count));
		if(Config.UserData.Instance.skillbar.Count > uiSkillFireThumbs.Length)
			Config.UserData.Instance.skillbar.RemoveRange(uiSkillFireThumbs.Length, Config.UserData.Instance.skillbar.Count - uiSkillFireThumbs.Length);
		for (var i = 0; i < uiSkillFireThumbs.Length; i++)
		{
			var button = uiSkillFireThumbs[i];
			UIEventListener.Get(button.gameObject).onClick = go =>
			{
				if(selected == null || selected.Skill.Value == null)
					return;
				button.normalSprite = selected.Skill.Value.icon;
			};
		}
		UIEventListener.Get(uiSkillFireOK.gameObject).onClick = go =>
		{
			if (selected == null || selected.Skill.Value == null)
				return;
			var index = System.Array.IndexOf(items, selected);
			Config.UserData.Instance.skillbar[index] = selected.Skill.Key;
			PresentFireThumbs();
		};
		PresentFireThumbs();

		// 更新事件
		SkillManager.Instance.SkillChanged += PresentIcons;
		PresentIcons(SkillManager.Instance);
	}

	void OnDestroy()
	{
		SkillManager.Instance.SkillChanged -= PresentIcons;
	}


	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
		PresentIcons(SkillManager.Instance);
	}

	void OnDisable()
	{
		selected = null;
	}

	/// <summary>
	/// 显示最上面的技能按钮
	/// </summary>
	/// <param name="manager"></param>
	private void PresentIcons(SkillManager manager)
	{
		if (this.gameObject.activeSelf == false || items == null)
			return;
		foreach (var i in items.Zip(manager.OrderBy(i => i.Key)))
			i.Item1.Skill = i.Item2;
	}

	/// <summary>
	/// 显示技能详细信息
	/// </summary>
	/// <param name="view"></param>
	private void PresentInfo(SkillInfoItem view)
	{
		selected = view;
		PresentFireThumbs();
		if (view == null || view.Skill.Value == null)
		{
			infoName.text = string.Empty;
			infoRequire.text = string.Empty;
			infoCD.text = string.Empty;
			infoDesc.text = string.Empty;
		}
		else
		{
			var s = view.Skill.Value;
			infoName.text = string.Format("[e28c00]{0}  {1}级[-]", s.name, s.level);
			infoRequire.text = string.Format("[e28c00]消耗真气: [-]{0}点  距离{1}码", s.requirePoint, s.radius);
			infoCD.text = string.Format("[e28c00]冷却时间: [-]{0}秒", s.cd);
			infoDesc.text = s.desc;
		}
	}

	/// <summary>
	/// 技能施法按钮显示状态和<see cref="Config.UserData.Instance"/>中的配置一致
	/// </summary>
	private void PresentFireThumbs()
	{
		for (var i = 0; i < uiSkillFireThumbs.Length; i++)
		{
			var skillID = Config.UserData.Instance.skillbar.Count > i ? Config.UserData.Instance.skillbar[i] : 0;
			var skill = SkillManager.Instance.GetSkill(skillID);
			var button = uiSkillFireThumbs[i];
			button.GetComponent<UISprite>().spriteName =
				button.disabledSprite =
				button.hoverSprite =
				button.pressedSprite =
				button.normalSprite =
				skill != null ? skill.icon : string.Empty;
			Debug.Log("PresentFireThumbs: " + button.normalSprite);
		}
	}
}
