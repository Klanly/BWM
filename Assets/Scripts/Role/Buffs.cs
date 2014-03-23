using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// <see cref="Buff"/>路径及其宿主的集合
/// </summary>
public class Buffs : MonoBehaviour
{

	public Dictionary<string, GameObject> buffs = new Dictionary<string, GameObject>();

	/// <summary>
	/// Adds the buff.
	/// </summary>
	/// <param name="buff">Buff路径</param>
	public void AddBuff(string strBuff)
	{
		var buff = Object.Instantiate(Resources.Load("Buff/" + strBuff)) as GameObject;
		buffs[strBuff] = buff.GetComponent<Buff>().StartBuff(gameObject);
	}

	/// <summary>
	/// Removes the buff.
	/// </summary>
	/// <param name="strBuff">String buff.</param>
	public void RemoveBuff(string strBuff)
	{
		if (buffs.ContainsKey(strBuff))
		{
			Destroy(buffs[strBuff]);
			buffs.Remove(strBuff);
		}
	}
}
