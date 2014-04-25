using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillCurveToTargetInSpeed : SendTargetEventBase
{
	public GameObject particle;
	public string mountOfStartGo;
	public string mountOfTargetGo;
	public float delay;
	public float speed;
	public Vector3 deviationDegree;
	public bool orientToPath = true;

	private GameObject particleGo;
	private Transform mountTargetGo;
	private Vector3[] path;
	private float movePosition = 0.0f;

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
			particleGo.transform.position = mountStartGo.transform.position;
			path = new Vector3[3];
			path[0] = mountStartGo.transform.position;

			state = State.move;
		}
		else if(state == State.move)
		{
			if (!mountTargetGo || !particleGo)
			{
				StartTargetEvent();
				return;
			}
			
			var distance = (mountTargetGo.transform.position - particleGo.transform.position).magnitude;
			if (distance <= 0.01f)
			{
				StartTargetEvent();
			}
			else
			{
				path[2] = mountTargetGo.transform.position;
				var relative = path[2] - path[0];
				var halfDis = relative.magnitude * 0.5f;
				path[1] = relative * 0.5f + new Vector3(halfDis * Mathf.Tan(Mathf.Deg2Rad * deviationDegree.x), halfDis * Mathf.Tan(Mathf.Deg2Rad * deviationDegree.y), halfDis * Mathf.Tan(Mathf.Deg2Rad * deviationDegree.z));
				var pathLength = Spline.PathLength(path);
				if (movePosition > pathLength)
					movePosition = pathLength;
				movePosition = Mathf.MoveTowards(movePosition, pathLength, speed * Time.deltaTime);
				if (movePosition > pathLength)
					movePosition = pathLength;
				if (movePosition >= pathLength)
				{
					StartTargetEvent();
				}
				else
				{
					var percentage = movePosition / pathLength;
					Vector3 curPos = Spline.InterpConstantSpeed(path, percentage);
					particleGo.transform.position = curPos;
					if(orientToPath)
						particleGo.transform.LookAt(Spline.InterpConstantSpeed(path, percentage + 0.05f));
				}
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
