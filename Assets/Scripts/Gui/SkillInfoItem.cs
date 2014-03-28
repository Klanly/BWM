using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillInfoItem : MonoBehaviour
{
	public UIButton uiIcon;
	public UILabel uiName;
	public UISprite uiUpgrade;

	private KeyValuePair<uint, table.TableSkill> m_tableInfo;
	public KeyValuePair<uint, table.TableSkill> Skill
	{
		get { return m_tableInfo; }
		set
		{
			m_tableInfo = value;
			Present();
		}
	}

	private void Present()
	{
		var s = Skill.Value ?? table.TableSkill.First(Skill.Key);
		uiName.text = s.name;
		uiIcon.normalSprite = s.icon;
		uiIcon.isEnabled = Skill.Value != null;
		// 避免SkillInfo关闭再打开，disable掉的按钮没有变灰。NGUI混色同步有bug
		uiIcon.defaultColor = uiIcon.isEnabled ? Color.white : Color.gray;
	}
}
