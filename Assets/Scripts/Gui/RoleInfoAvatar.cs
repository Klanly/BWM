using UnityEngine;
using System.Collections;

public class RoleInfoAvatar : MonoBehaviour
{
	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}
}
