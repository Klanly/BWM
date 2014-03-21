using UnityEngine;
using System.Collections;

public class TestSkill : MonoBehaviour {

	public GameObject startGo;
	public GameObject targetGo;
	public GameObject skillGo;

	void Start()
	{
		startSkill();
	}

	[ContextMenu ("start skill")]
	void startSkill()
	{
		var skill = Object.Instantiate(skillGo) as GameObject;
		skill.GetComponent<Skill>().startGo = startGo;
		skill.GetComponent<Skill>().targetGo = targetGo;

		foreach(SkillBase t in skill.GetComponents<SkillBase>())
		{
			t.StartSkill();
		}
		skill.transform.localPosition = Vector3.zero;
		skill.transform.parent = startGo.transform;
	}
}
