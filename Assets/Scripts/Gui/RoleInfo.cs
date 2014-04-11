using UnityEngine;
using System.Collections;

public class RoleInfo : MonoBehaviour
{
	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}
}
