using UnityEngine;
using System.Collections;

public class RoleInfo : MonoBehaviour
{
	public UIButton closeButton;

	void Start()
	{
		UIEventListener.Get(closeButton.gameObject).onClick = go => this.gameObject.SetActive(false);
	}
	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}
}
