﻿using UnityEngine;
using System.Collections;

public class TestJumpPos : MonoBehaviour
{

	public string nextLevel;
	public float distance;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (string.IsNullOrEmpty(nextLevel) || MainRole.Instance == null)
			return;
		Transform role = MainRole.Instance.gameObject.transform;
		Vector2 posRole = new Vector2(role.position.x, role.position.z);
		Vector2 posSelf = new Vector2(transform.position.x, transform.position.z);
		if (Vector2.Distance(posRole, posSelf) < distance)
		{
			//Object.FindObjectOfType<MainRole>().LoadMap(nextLevel);
			// TODO: send jump message to server
		}
	}
}
