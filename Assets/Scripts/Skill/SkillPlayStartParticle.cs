using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillPlayStartParticle : SkillBase
{
	public float delay;
	public GameObject particle;
	public string mountOfStartGo;

	override public void StartSkill()
	{
		if (delay > 0.0f)
		{
			iTween.ValueTo(gameObject, iTween.Hash(
				"from", delay, 
				"to", 0.0f, 
				"time", delay, 
				"onupdate", "onUpdate_SkillPlayStartParticle",
				"oncomplete", "PlayParticle_SkillPlayStartParticle"));
		}
		else
		{
			PlayParticle_SkillPlayStartParticle();
		}
	}

	void onUpdate_SkillPlayStartParticle(float delay) { }

	void PlayParticle_SkillPlayStartParticle()
	{
		if (particle == null)
		{
			Destroy(this);
			return;
		}
		
		var skill = gameObject.GetComponent<Skill>();
		if (skill && skill.startGo)
		{
			var mount = SkillBase.Find(skill.startGo.transform, mountOfStartGo);
			if (!mount)
				mount = skill.startGo.transform;
			
			var particleGo = Instantiate(particle) as GameObject;
			if(particleGo.GetComponent<ParticleParentAutoDestroy>() == null)
				particleGo.AddComponent<ParticleParentAutoDestroy>();
			particleGo.transform.parent = mount;
			particleGo.transform.localPosition = Vector3.zero;
		}
		
		Destroy(this);
	}
}
