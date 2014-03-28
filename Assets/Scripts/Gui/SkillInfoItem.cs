using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillInfoItem : MonoBehaviour
{
	public UISprite uiIcon;
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
		var skill = Skill.Value ?? table.TableSkill.First(Skill.Key);
		uiIcon.spriteName = skill.icon;
		uiName.text = skill.name;
		this.gameObject.GetComponent<UIButton>().isEnabled = Skill.Value != null;
	}
}
