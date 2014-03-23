using UnityEngine;
using System.Collections;

/// <summary>
/// 可指定目标释放技能
/// </summary>
public class CastSkill : MonoBehaviour
{

	/// <summary>
	/// Starts the skill.
	/// </summary>
	/// <param name="strSkill">技能路径</param>
	/// <param name="targetGo">技能释放的目标</param>
	public void StartSkill(string strSkill, GameObject targetGo)
	{
		var skill = Object.Instantiate(Resources.Load("Skill/" + strSkill)) as GameObject;
		skill.GetComponent<Skill>().startGo = gameObject;
		skill.GetComponent<Skill>().targetGo = targetGo;

		foreach (SkillBase t in skill.GetComponents<SkillBase>())
		{
			t.StartSkill();
		}
		skill.transform.parent = transform;
		skill.transform.localPosition = Vector3.zero;
	}
}
