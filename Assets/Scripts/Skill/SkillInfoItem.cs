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
	}

	public override string ToString()
	{
		return Skill.Value.ToString();
	}
}
