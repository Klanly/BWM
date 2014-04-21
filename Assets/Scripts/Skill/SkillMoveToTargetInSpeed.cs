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

	/// <summary>
	/// 运动状态
	/// </summary>
	private enum State
	{
		delay,
		start,
		move,
	}
	private State state = State.delay;

	override public void StartSkill()
	{
	}

	void Update()
	{
		if(state == State.delay)
		{
			delay -= Time.deltaTime;
			if(delay <= 0.0f)
				state = State.start;
		}

		if(state == State.start)
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
			if(particleGo.GetComponent<ParticleParentAutoDestroy>() == null)
				particleGo.AddComponent<ParticleParentAutoDestroy>();
			particleGo.transform.localPosition = Vector3.zero;
			particleGo.transform.position = mountStartGo.transform.position;

			state = State.move;
		}
		else if(state == State.move)
		{
			if (!mountTargetGo || !particleGo)
			{
				StartTargetEvent();
				return;
			}

			particleGo.transform.position = Vector3.MoveTowards(particleGo.transform.position, mountTargetGo.transform.position, speed * Time.deltaTime);
			if(orientToPath)
				particleGo.transform.LookAt(mountTargetGo.transform);

			var relative = mountTargetGo.transform.position - particleGo.transform.position;
			if (relative.magnitude <= 0.01f)
			{
				StartTargetEvent();
			}
		}
	}

	void StartTargetEvent()
	{
		if (particleGo != null)
		{
			if(immediateDeleteParticle)
			{
				Destroy(particleGo);
			}
			else
			{
				particleGo.GetComponent<ParticleParentAutoDestroy>().SetOnce();
			}
		}
		if (sendTargetEvent)
			gameObject.SendMessage("ApplyTargetEvent");
		Destroy(this);
	}
}
