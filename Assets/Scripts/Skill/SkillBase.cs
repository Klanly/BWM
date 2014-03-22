using UnityEngine;
using System.Collections;

public class SkillBase : MonoBehaviour {

	/// <summary>
	/// Starts the skill.
	/// </summary>
	virtual public void StartSkill()
	{
	}

	/// <summary>
	/// 从所有子节点中找出相应的子节点
	/// </summary>
	/// <param name="parent">Parent.</param>
	/// <param name="childname">Childname.</param>
	public static Transform Find(Transform parent, string childname)
	{
		Transform[] allchild = parent.GetComponentsInChildren<Transform>();
		foreach(Transform t in allchild)
		{
			if (t.name == childname)
			{
				return t;
			}
		}
		return null;
	}
}
