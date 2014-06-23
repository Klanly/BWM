using UnityEngine;
using System.Collections;

/// <summary>
/// 延迟给定时间后绘制伤害
/// </summary>
[RequireComponent(typeof(Skill))]
public class SkillDrawHurt : SkillBase
{
	public float delay;

	IEnumerator ApplyTargetEvent()
	{
		if (delay > 0.0f)
		{
			yield return new WaitForSeconds (delay);
			DrawHurt();
		}
		else
		{
			DrawHurt();
		}
	}

	/// <summary>
	/// 绘制伤害
	/// </summary>
	void DrawHurt()
	{
		var skill = gameObject.GetComponent<Skill>();
		if(skill)
		{
			for (int i = 0; i < skill.hurts.Count; ++i)
			{
				var t = skill.hurts[i];
				var targetGo = skill.targetGos[i];
                var hpproto = targetGo.GetComponent<HpProtocol>();
                if (hpproto.hp == t.prehp)
                    hpproto.hp = hpproto.hp - t.subhp;
				
				if (targetGo != null)
				{
					Debug.Log(skill.TableInfo.name + ":(" + skill.TableInfo.path + "):" + targetGo.name + ":hp:" + t.subhp);

					var gohp = Instantiate(Resources.Load("Prefabs/Gui/HurtTipHp")) as GameObject;
					var labelhp = gohp.GetComponentInChildren<UILabel>();
					if (t.subhp > 0)
						labelhp.text = "-" + t.subhp.ToString();
					else if (t.subhp < 0)
						labelhp.text = "+" + (-1 * t.subhp).ToString();
					gohp.GetComponent<UIWidget>().SetAnchor(targetGo);
				}
			}
		}

		Destroy(this);
	}
}
