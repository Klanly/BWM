using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillPlayTargetParticle : SkillBase
{

	public float delay;
	public GameObject particle;
	public string mountOfTargetGo;

	void ApplyTargetEvent()
	{
		if (delay > 0.0f)
		{
			iTween.ValueTo(gameObject, iTween.Hash(
				"from", delay, 
				"to", 0.0f, 
				"time", delay, 
				"onupdate", "onUpdate",
				"oncomplete", "PlayParticle"));
		}
		else
		{
			PlayParticle();
		}
	}

	void onUpdate(float delay) { }

	void PlayParticle()
	{
		if(particle == null)
		{
			Destroy(this);
			return;
		}

		var skill = gameObject.GetComponent<Skill>();
		if (skill && skill.targetGo)
		{
			var mount = SkillBase.Find(skill.targetGo.transform, mountOfTargetGo);
			if (!mount)
				mount = skill.targetGo.transform;

			var particleGo = Instantiate(particle) as GameObject;
			if(particleGo.GetComponent<ParticleParentAutoDestroy>() == null)
				particleGo.AddComponent<ParticleParentAutoDestroy>();
			particleGo.transform.parent = mount;
			particleGo.transform.localPosition = Vector3.zero;
		}

		Destroy(this);
	}
}
