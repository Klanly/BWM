using UnityEngine;
using System.Collections;

/// <summary>
/// 发送到达目标消息的插件的基类，用于统计一个技能中不能有多个插件发送这个消息
/// </summary>
public class SendTargetEventBase : SkillBase {

	/// <summary>
	/// 到达目标后是否发送消息。一个技能中只能有一个组件标记为true
	/// </summary>
	public bool sendTargetEvent = false;


	/// <summary>
	/// 到达目标后是否立即删除运动的特效
	/// </summary>
	public bool immediateDeleteParticle = false;
}
