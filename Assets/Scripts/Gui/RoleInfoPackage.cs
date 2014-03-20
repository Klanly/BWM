using UnityEngine;
using System.Collections;

public class RoleInfoPackage : MonoBehaviour
{
	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);

		Debug.Log(ItemManager.Instance.ToString());
	}
}
