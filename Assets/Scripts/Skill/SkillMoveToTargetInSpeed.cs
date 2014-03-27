using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillMoveToTargetInSpeed : SendTargetEventBase
{
	public GameObject particle;
	public string mountOfStartGo;
	public string mountOfTargetGo;
	public float delay;
	public float speed;
	public bool orientToPath = true;

	private GameObject particleGo;
	private Transform mountTargetGo;
	private bool startMove = false;

	override public void StartSkill()
	{
		if (delay > 0.0f)
		{
			iTween.ValueTo(gameObject, iTween.Hash(
				"from", delay, 
				"to", 0.0f, 
				"time", delay, 
				"onupdate", "onUpdate", 
				"oncomplete", "MoveParticle"));
		}
		else
		{
			MoveParticle();
		}
	}

	void onUpdate(float delay) { }

	void MoveParticle()
	{
		var skill = gameObject.GetComponent<Skill>();
		if (!skill || !skill.startGo || !skill.targetGo)
		{
			StartTargetEvent();
			return;
		}

		var mountStartGo = SkillBase.Find(skill.startGo.transform, mountOfStartGo);
		if (!mountStartGo)
			mountStartGo = skill.startGo.transform;

		mountTargetGo = SkillBase.Find(skill.targetGo.transform, mountOfTargetGo);
		if (!mountTargetGo)
			mountTargetGo = skill.targetGo.transform;

		particleGo = Instantiate(particle) as GameObject;
		particleGo.transform.localPosition = Vector3.zero;
		particleGo.transform.position = mountStartGo.transform.position;
		startMove = true;
	}

	void Update()
	{
		if (startMove)
		{
			if (!mountTargetGo || !particleGo)
			{
				StartTargetEvent();
				return;
			}

			var relative = mountTargetGo.transform.position - particleGo.transform.position;
			var distance = relative.magnitude;
			if (distance <= 0.01f)
			{
				StartTargetEvent();
			}
			else
			{
				var time = distance / speed;
				iTween.MoveUpdate(particleGo, iTween.Hash(
					"position", mountTargetGo, 
					"orienttopath", orientToPath, 
					"time", time));
			}
		}
	}

	void StartTargetEvent()
	{
		if (particleGo != null)
		{
			foreach (ParticleSystem t in particleGo.GetComponentsInChildren<ParticleSystem>())
				t.loop = false;
		}
		if (sendTargetEvent)
			gameObject.SendMessage("ApplyTargetEvent");
		Destroy(this);
	}
}
