using UnityEngine;
using System.Collections;

public class SkillBase : MonoBehaviour
{
	virtual public void StartSkill()
	{
	}

	/// <summary>
	/// 递归找出指定名称的子节点
	/// </summary>
	/// <param name="parent">Parent.</param>
	/// <param name="childname">Childname.</param>
	public static Transform Find(Transform parent, string childname)
	{
		Transform[] allchild = parent.GetComponentsInChildren<Transform>();
		foreach (Transform t in allchild)
		{
			if (t.name == childname)
			{
				return t;
			}
		}
		return null;
	}
}
