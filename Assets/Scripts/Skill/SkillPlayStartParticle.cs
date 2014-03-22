using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillPlayStartParticle : SkillBase {

	public GameObject particle;
	public string mountOfStartGo;
	
	override public void StartSkill()
	{
		if(particle == null)
			return;
		
		var skill = gameObject.GetComponent<Skill>();
		if(skill && skill.startGo)
		{
			var mount = SkillBase.Find(skill.startGo.transform, mountOfStartGo);
			if(!mount)
				mount = skill.startGo.transform;

			var par = Instantiate(particle) as GameObject;
			par.transform.parent = mount;
			par.transform.localPosition = Vector3.zero;
		}

		Destroy(this);
	}
}
