using UnityEngine;
using System.Collections;

public class ParticleAndAnimation : MonoBehaviour
{
	void Start () 
	{
	}
	
	[ContextMenu("Play Loop")]
	public void PlayLoop()
	{
		ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>(true);
		foreach(ParticleSystem ps in pss)
		{
			ps.loop = true;
			ps.Play();
		}
		Animation[] anis = GetComponentsInChildren<Animation>(true);
		foreach(Animation an in anis)
		{
			an.wrapMode = WrapMode.Loop;
			an.Play();
		}
		Animator[] animators = GetComponentsInChildren<Animator>(true);
		foreach(Animator an2 in animators)
		{
			an2.CrossFade("end", 0);
			an2.Play("start");
		}
	}
	
	[ContextMenu("Play Once")]
	public void PlayOnce () 
	{	
		ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>(true);
		foreach(ParticleSystem ps in pss)
		{
			ps.loop = false;
			ps.Play();
		}
		Animation[] anis = GetComponentsInChildren<Animation>(true);
		foreach(Animation an in anis)
		{
			an.wrapMode = WrapMode.Once;
			an.Play();
		}
		Animator[] animators = GetComponentsInChildren<Animator>(true);
		foreach(Animator an2 in animators)
		{
			an2.CrossFade("end", 0);
			an2.Play("start");
		}
	}
}
