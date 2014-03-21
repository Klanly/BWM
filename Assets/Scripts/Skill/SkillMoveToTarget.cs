using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillMoveToTarget : SkillBase {

	public GameObject particle;
	public string mountOfStartGo;
	public string mountOfTargetGo;
	public float delay;

	/// <summary>
	/// 移动时间
	/// </summary>
	public float time;

	// Use this for initialization
	override public void StartSkill () {
		var skill = gameObject.GetComponent<Skill>();
		if(skill && skill.startGo && skill.targetGo)
		{
			var mountStart = skill.startGo.transform.Find(mountOfStartGo);
			if(!mountStart)
				mountStart = skill.startGo.transform;
			
			var mountTarget = skill.targetGo.transform.Find(mountOfTargetGo);
			if(!mountTarget)
				mountTarget = skill.targetGo.transform;
			
			var par = Instantiate(particle) as GameObject;
			iTween.MoveTo(par, iTween.Hash("path", new Transform[]{mountStart, mountTarget}, "movetopath", false, "orienttopath", true, "time", time, "delay", delay, "oncomplete", "StartTargetEvent", "oncompletetarget", gameObject));
		}
	}

	void StartTargeEvent()
	{
		gameObject.SendMessage("ApplyTargetEvent");
		Destroy(this);
	}
}
