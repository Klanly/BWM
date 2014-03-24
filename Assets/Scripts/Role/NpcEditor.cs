using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Entity))]
public class NpcEditor : MonoBehaviour
{
	/// <summary>
	/// NPC表中的ID
	/// </summary>
	public int baseId;
	/// <summary>
	/// 别名
	/// </summary>
	public string alias;
	/// <summary>
	/// 重生时间(秒)
	/// </summary>
	public int relivetime;
	/// <summary>
	/// 刷新概率
	/// </summary>
	public int rate = 100;
}
