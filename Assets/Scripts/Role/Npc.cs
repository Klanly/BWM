using UnityEngine;
using System.Collections;

public class Npc : MonoBehaviour
{
	/// <summary>
	/// NPC表中的ID
	/// </summary>
	public int baseId;
	/// <summary>
	/// 别用
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
