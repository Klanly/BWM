using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillMoveToTargetInTime : SendTargetEventBase
{
	public GameObject particle;
	public string mountOfStartGo;
	public string mountOfTargetGo;
	public float delay;
	public float time;
	public Easing.EaseType easeType = Easing.EaseType.linear;
	public bool orientToPath = true;

	private GameObject particleGo;
	private Transform mountTargetGo;
	private Vector3[] path;
	private float curTime;

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

	// Use this for initialization
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
			path = new Vector3[2] { mountStartGo.transform.position, mountTargetGo.transform.position };
			curTime = 0.0f;

			state = State.move;
		}

		else if(state == State.move)
		{
			curTime += Time.deltaTime;
			float percentage = curTime / time;
			if(percentage >= 1.0f)
			{
				StartTargetEvent();
			}

			if (particleGo && mountTargetGo)
			{
				path[1] = mountTargetGo.transform.position;
				Vector3 curPos = Spline.InterpConstantSpeed(path, percentage, easeType);
				particleGo.transform.position = curPos;
				if(orientToPath)
					particleGo.transform.LookAt(Spline.InterpConstantSpeed(path, percentage + 0.05f, easeType));
			}
		}
	}

	void StartTargetEvent()
	{
		if (particleGo != null)
		{
			if(immediateDeleteParticle)
				Destroy(particleGo);
			else
				particleGo.GetComponent<ParticleParentAutoDestroy>().SetOnce();
		}
		if (sendTargetEvent)
			gameObject.SendMessage("ApplyTargetEvent");
		Destroy(this);
	}
}
