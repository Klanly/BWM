using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillMoveToTargetInTime : SkillBase
{

	public bool sendTargetEvent = false;
	public GameObject particle;
	public string mountOfStartGo;
	public string mountOfTargetGo;
	public float delay;
	public float time;
	public iTween.EaseType easeType = iTween.EaseType.linear;
	public bool orientToPath = true;

	private GameObject particleGo;
	private Transform mountTargetGo;
	private Vector3[] path;

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
		path = new Vector3[2] { mountStartGo.transform.position, mountTargetGo.transform.position };
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 0.0f, 
			"to", time, 
			"time", time, 
			"easetype", easeType, 
			"onupdate", "onMoveUpdate",
			"oncomplete", "StartTargetEvent"));
	}

	void onMoveUpdate(float curTime)
	{
		if (!particleGo || !mountTargetGo)
			return;

		path[1] = mountTargetGo.transform.position;
		float percentage = curTime / time;
		iTween.PutOnPath(particleGo, path, percentage);
		if (orientToPath)
			particleGo.transform.LookAt(iTween.PointOnPath(path, percentage + .05f));
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
