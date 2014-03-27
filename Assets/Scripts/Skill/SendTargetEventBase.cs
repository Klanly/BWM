using UnityEngine;
using System.Collections;

/// <summary>
/// 发送到达目标消息的插件的基类，用于统计一个技能中不能有多个插件发送这个消息
/// </summary>
public class SendTargetEventBase : SkillBase {
	public bool sendTargetEvent = false;
}
