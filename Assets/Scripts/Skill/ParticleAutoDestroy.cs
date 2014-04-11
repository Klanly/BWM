using UnityEngine;
using System.Collections;

public class ParticleAutoDestroy : MonoBehaviour
{
	private Animator animator;
	private bool isAddEvent = false;
	
	void Start()
	{
		animator = gameObject.GetComponent<Animator>();
		if(animation && !animation.isPlaying)
			Destroy(animation);
	}
	
	// Update is called once per frame
	void Update()
	{
		if(animator)
		{
			AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
			if(info.IsName("end"))
			{
				Destroy(animator);
				animator = null;
			}
		}
		
		if(particleSystem)
		{
			if (!particleSystem.IsAlive()) Destroy(particleSystem);
		}
		
		if(animation && (animation.wrapMode == WrapMode.Once || animation.clip.wrapMode == WrapMode.Once) && !isAddEvent)
		{
			isAddEvent = true;
			animation.clip.AddEvent(new AnimationEvent() {functionName = "DestroyAnimation", time=animation.clip.length});
		}
		
		if(!animator && !particleSystem && !animation)
			Destroy(gameObject);
	}
	
	void DestroyAnimation()
	{
		Destroy(animation);
	}
}
