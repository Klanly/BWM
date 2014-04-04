using UnityEngine;
using System.Collections;

public class ParticleAutoDestroy : MonoBehaviour
{
	private bool isAddEvent = false;

	void Start()
	{
		if(animation && !animation.isPlaying)
			Destroy(animation);
	}

	// Update is called once per frame
	void Update()
	{
		if(particleSystem)
		{
			if (!particleSystem.IsAlive()) Destroy(particleSystem);
		}

		if(animation && (animation.wrapMode == WrapMode.Once || animation.clip.wrapMode == WrapMode.Once) && !isAddEvent)
		{
			isAddEvent = true;
			animation.clip.AddEvent(new AnimationEvent() {functionName = "DestroyAnimation", time=animation.clip.length});
		}

		if(!particleSystem && !animation)
			Destroy(gameObject);
	}

	void DestroyAnimation()
	{
		Destroy(animation);
	}
}
