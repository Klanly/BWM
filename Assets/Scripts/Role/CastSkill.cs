using UnityEngine;
using System.Collections;

public class CastSkill : MonoBehaviour {

	/// <summary>
	/// Starts the skill.
	/// </summary>
	/// <param name="strSkill">skill name.</param>
	/// <param name="targetGo">Target go.</param>
	void StartSkill(string strSkill, GameObject targetGo)
	{
		var skill = Object.Instantiate(Resources.Load("Skill/" + strSkill)) as GameObject;
		skill.GetComponent<Skill>().startGo = gameObject;
		skill.GetComponent<Skill>().targetGo = targetGo;
		
		foreach(SkillBase t in skill.GetComponents<SkillBase>())
			t.StartSkill();

		skill.transform.localPosition = Vector3.zero;
		skill.transform.parent = gameObject.transform;
	}
}
