using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillPlayTargetParticle : SkillBase
{
	public float delay;
	public GameObject particle;
	public string mountOfTargetGo;

	IEnumerator ApplyTargetEvent()
	{
		if (delay > 0.0f)
		{
			yield return new WaitForSeconds (delay);
			PlayParticle();
		}
		else
		{
			PlayParticle();
		}
	}

	void PlayParticle()
	{
		if(particle == null)
		{
			Destroy(this);
			return;
		}

		var skill = gameObject.GetComponent<Skill>();
		if (skill)
		{
			foreach(var targetGo in skill.targetGos)
			{
				if (targetGo != null)
				{
					var mount = SkillBase.Find(targetGo.transform, mountOfTargetGo);
					if (!mount)
						mount = targetGo.transform;
					
					var particleGo = Instantiate(particle) as GameObject;
					if(particleGo.GetComponent<ParticleParentAutoDestroy>() == null)
						particleGo.AddComponent<ParticleParentAutoDestroy>();
					particleGo.transform.parent = mount;
					particleGo.transform.localPosition = Vector3.zero;
					particleGo.transform.localRotation = Quaternion.identity;
				}
			}
		}

		Destroy(this);
	}
}
