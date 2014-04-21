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
	}

	void Update()
	{
		delay -= Time.deltaTime;
		if (delay <= 0.0f)
		{
			PlayParticle();
		}
	}

	void PlayParticle()
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
