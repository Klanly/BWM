using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Skill))]
public class SkillCurveToTargetInTime : SkillBase {

	public bool sendTargetEvent = false;
	public GameObject particle;
	public string mountOfStartGo;
	public string mountOfTargetGo;
	public float delay;
	public float time;
	public Vector3 deviationDegree;
	public iTween.EaseType easeType = iTween.EaseType.linear;
	public bool orientToPath = true;

	private GameObject particleGo;
	private Transform mountTargetGo;
	private Vector3[] path;

	// Use this for initialization
	override public void StartSkill () {
		if(delay > 0.0f)
		{
			iTween.ValueTo(gameObject, iTween.Hash("from",delay,"to",0.0f,"time",delay,"onupdate", "onUpdate", "oncomplete","MoveParticle"));
		}
		else
		{
			MoveParticle();
		}		
	}

	void onUpdate(float delay) {}

	void MoveParticle()
	{
		var skill = gameObject.GetComponent<Skill>();
		if(skill && skill.startGo && skill.targetGo)
		{
			var mountStartGo = skill.startGo.transform.Find(mountOfStartGo);
			if(!mountStartGo)
				mountStartGo = skill.startGo.transform;
			
			mountTargetGo = skill.targetGo.transform.Find(mountOfTargetGo);
			if(!mountTargetGo)
				mountTargetGo = skill.targetGo.transform;
			
			particleGo = Instantiate(particle) as GameObject;
			particleGo.transform.localPosition = Vector3.zero;
			path = new Vector3[3];
			path[0] = mountStartGo.transform.position;
			iTween.ValueTo(gameObject, iTween.Hash("from",0.0f,"to",time,"time",time,"easetype",easeType,"onupdate", "onMoveUpdate", "oncomplete","StartTargetEvent"));
		}
	}

	void onMoveUpdate(float curTime)
	{
		path[2] = mountTargetGo.transform.position;
		var relative = path[2] - path[0];
		var halfDis = relative.magnitude * 0.5f;
		path[1] = relative * 0.5f + new Vector3( halfDis * Mathf.Tan(Mathf.Deg2Rad * deviationDegree.x), halfDis * Mathf.Tan(Mathf.Deg2Rad * deviationDegree.y), halfDis * Mathf.Tan(Mathf.Deg2Rad * deviationDegree.z) );

		float percentage = curTime / time;
		iTween.PutOnPath( particleGo, path, percentage );
		if(orientToPath)
			particleGo.transform.LookAt(iTween.PointOnPath(path, percentage+.05f));
	}

	void StartTargetEvent()
	{
		particleGo.particleSystem.loop = false;
		if(sendTargetEvent)
			gameObject.SendMessage("ApplyTargetEvent");
		Destroy(this);
	}
}
