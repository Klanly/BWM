using UnityEngine;
using System.Collections;

/// <summary>
/// 可指定目标释放技能
/// </summary>
public class CastSkill : MonoBehaviour
{

	/// <summary>
	/// Starts the skill.
	/// </summary>
	/// <param name="strSkill">技能prefab的全路径</param>
	/// <param name="targetGo">技能释放的目标</param>
	public bool StartSkill(string strSkill, GameObject targetGo)
	{
		var res = Resources.Load(strSkill);
		if (res == null)
		{
			Debug.LogError("无法加载技能文件: " + strSkill);
			return false;
		}
		var skill = Object.Instantiate(res) as GameObject;
		skill.GetComponent<Skill>().startGo = gameObject;
		skill.GetComponent<Skill>().targetGo = targetGo;

		// 检查技能的有效性
		int count = 0;
		foreach(SendTargetEventBase t in skill.GetComponents<SendTargetEventBase>())
		{
			if (t.sendTargetEvent == true)
				count ++;
			
			if(count >= 2)
			{
				Debug.LogError("技能(" + strSkill + ")有多个发送到达目标的组件");
				return false;
			}
		}

		foreach (SkillBase t in skill.GetComponents<SkillBase>())
		{
			t.StartSkill();
		}
		skill.transform.parent = transform;
		skill.transform.localPosition = Vector3.zero;
		return true;
	}
}
