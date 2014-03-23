using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillCurveToTargetInSpeed : SkillBase
{

	public bool sendTargetEvent;
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
	private bool startMove = false;
	private float movePosition = 0.0f;

	// Use this for initialization
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
		path = new Vector3[3];
		path[0] = mountStartGo.transform.position;
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
				var pathLength = iTween.PathLength(path);
				if (movePosition > pathLength)
					movePosition = pathLength;
				movePosition = iTween.FloatUpdate(movePosition, pathLength, speed);
				if (movePosition > pathLength)
					movePosition = pathLength;
				if (movePosition >= pathLength)
				{
					StartTargetEvent();
				}
				else
				{
					var percentage = movePosition / pathLength;
					iTween.PutOnPath(particleGo, path, percentage);
					if (orientToPath)
						particleGo.transform.LookAt(iTween.PointOnPath(path, percentage + .05f));
				}
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
