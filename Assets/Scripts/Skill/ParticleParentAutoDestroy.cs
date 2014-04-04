using UnityEngine;
using System.Collections;

public class ParticleParentAutoDestroy : MonoBehaviour
{
	void Start()
	{
		foreach(Transform t in transform)
		{
			if (t.gameObject.GetComponent<ParticleAutoDestroy>() == null)
			{
				t.gameObject.AddComponent<ParticleAutoDestroy>();
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (GetComponentsInChildren<ParticleSystem>().Length == 0 && GetComponentsInChildren<Animator>().Length == 0 && GetComponentsInChildren<Animation>().Length == 0)
			Destroy(gameObject);
	}

	/// <summary>
	/// 设置所有子节点的特效和动画的loop为false
	/// </summary>
	public void SetOnce()
	{
		foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
		{
			ps.loop = false;
		}

		foreach(Animator animator in GetComponentsInChildren<Animator>())
		{
			foreach(AnimationInfo info in animator.GetCurrentAnimationClipState(0))
			{
				info.clip.wrapMode = WrapMode.Once;
			}
		}

		foreach(Animation ani in GetComponentsInChildren<Animation>())
		{
			ani.clip.wrapMode = WrapMode.Once;
			ani.wrapMode = WrapMode.Once;
		}
	}
}
