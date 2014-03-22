using UnityEngine;
using System.Collections;

/// <summary>
/// Buff
/// </summary>
public class Buff : MonoBehaviour {

	public int buffId;
	public GameObject particleGo;
	public string mount;

	/// <summary>
	/// Starts the buff,创建一个特效挂载在目标身上
	/// </summary>
	/// <param name="target">Target.</param>
	public GameObject StartBuff(GameObject target)
	{
		if(!target)
			return null;

		var mountGo = SkillBase.Find(target.transform, mount);
		if(!mountGo)
			mountGo = target.transform;
		GameObject particle = Object.Instantiate(particleGo) as GameObject;
		if(particle)
		{
			particle.transform.parent = mountGo;
			particle.transform.localPosition = Vector2.zero;
		}

		Destroy(this.gameObject);
		return particle;
	}
}
