using UnityEngine;
using System.Collections;

public class SkillView : MonoBehaviour {

	public GameObject startGo;
	public GameObject targetGo;
	public GameObject skillGo;
	public GameObject buffGo;

	void Start()
	{
		//startSkill();
	}

	[ContextMenu ("start skill")]
	public void startSkill()
	{
		var skill = Object.Instantiate(skillGo) as GameObject;
		skill.GetComponent<Skill>().startGo = startGo;
		skill.GetComponent<Skill>().targetGo = targetGo;
		skill.GetComponent<Skill>().targetGos.Add(targetGo);
		Cmd.SkillHurtData hurt = new Cmd.SkillHurtData();
		hurt.subhp = 100;
		skill.GetComponent<Skill>().hurts.Add(hurt);

		// 检查技能的有效性
		int count = 0;
		foreach(SendTargetEventBase t in skill.GetComponents<SendTargetEventBase>())
		{
			if (t.sendTargetEvent == true)
				count ++;

			if(count >= 2)
			{
				Debug.LogError("技能有多个发送到达目标的组件");
				return;
			}
		}

		foreach(SkillBase t in skill.GetComponents<SkillBase>())
		{
			t.StartSkill();
		}
		skill.transform.parent = startGo.transform;
		skill.transform.localPosition = Vector3.zero;
	}

	[ContextMenu ("start buff")]
	public void startBuff()
	{
		var buff = Object.Instantiate(buffGo) as GameObject;
		buff.GetComponent<Buff>().StartBuff(startGo);
	}
}
