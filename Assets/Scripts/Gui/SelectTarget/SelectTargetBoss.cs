using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class SelectTargetBoss : MonoBehaviour
{
	public UILabel nameLabel;
	public UISprite uiHp;
	public UISprite uiHpBack;
	private List<string> bars = new List<string>();

	internal void OnSelect(Npc target)
	{
		nameLabel.text = target.TableInfo.name;
		SetupBars(target.TableInfo.hpBar);
	}
	internal void OnUpdate(Npc target)
	{
		OnHpUpdate(target.ServerInfo.hp / (float)target.ServerInfo.maxhp);
	}

	/// <summary>
	/// 设置血条贴图序列
	/// </summary>
	/// <param name="sprites">最先显示的在最前面</param>
	void SetupBars(IEnumerable<string> sprites)
	{
		bars.Clear();
		bars.Add(string.Empty); // 空管毛都不显示的占位符
		bars.AddRange(sprites.Reverse());
	}

	void OnHpUpdate(float percent)
	{
		percent = Mathf.Clamp01(percent);
		var count = bars.Count - 1.0f;
		var index = Mathf.FloorToInt(percent * count);
		var remainder = percent - index / count;

		uiHpBack.spriteName = bars[index];
		uiHp.spriteName = bars[index + 1];
		uiHp.fillAmount = remainder;
	}
}
