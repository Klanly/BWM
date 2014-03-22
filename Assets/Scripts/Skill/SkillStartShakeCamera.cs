using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Skill))]
public class SkillStartShakeCamera : SkillBase {

	public float delay;
	public Vector3 shakeAmount;
	public float time;
	
	override public void StartSkill()
	{
		var cameraGo = Camera.main.gameObject;
		if(cameraGo != null)
		{
			iTween.ShakePosition(cameraGo, iTween.Hash("amount", shakeAmount, "time", time, "delay", delay));
		}
		Destroy(this);
	}
	
}
