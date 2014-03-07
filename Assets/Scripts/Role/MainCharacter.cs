﻿using UnityEngine;
using System.Collections;
using Cmd;
using GX;
using GX.Net;
using MainCharacterInfo = Cmd.FirstMainUserDataAndPosMapUserCmd_S;

public class MainCharacter : MonoBehaviour
{
	public static MainCharacterInfo ServerInfo { get; private set; }

	public float speedMainRole = 5.0f;
	public float distanceCameraToRole = 25.0f;
	public float heightCameraLookAt = 0.5f;

	public MapNav MapNav { get; private set; }

	private Transform mainRole;
	private Transform birthPos;

	/// <summary>
	/// 主角世界坐标位置
	/// </summary>
	public Vector3 Position
	{
		get { return mainRole.position; }
		set 
		{
			mainRole.position = value;

			// 设置照相机位置
			Vector3 targetCenter = mainRole.position;
			targetCenter.y += heightCameraLookAt;
			this.transform.position = targetCenter;
			this.transform.position += this.transform.rotation * Vector3.back * distanceCameraToRole;
		}
	}

	/// <summary>
	/// 主角逻辑格子位置
	/// </summary>
	public GridPosition Grid
	{
		get { return new GridPosition() { X = MapNav.GetGridX(Position), Z = MapNav.GetGridZ(Position) }; }
		set { Position = MapNav.GetWorldPosition(value.X, value.Z); }
	}

	static MainCharacter()
	{
		ServerInfo = new MainCharacterInfo(); // 避免不必要的空指针判断
	}

	// Use this for initialization
	void Start()
	{
		MapNav = Object.FindObjectOfType<MapNav>();
	}

	// Update is called once per frame
	void Update()
	{
		if (mainRole == null || birthPos == null)
		{
			if (GameObject.Find("BirthPos"))
				birthPos = GameObject.Find("BirthPos").transform;
			if (GameObject.Find("MainRole"))
				mainRole = GameObject.Find("MainRole").transform;
			if (mainRole && birthPos)
				Position = birthPos.position;
		}


		// 设置主角的移动
		if (mainRole)
		{
			// keyboard
			float v = Input.GetAxisRaw("Vertical");
			float h = Input.GetAxisRaw("Horizontal");

			Vector3 oldPosition = mainRole.transform.position;
			oldPosition.x += h * speedMainRole * Time.deltaTime;
			oldPosition.z += v * speedMainRole * Time.deltaTime;
			Position = oldPosition;

			// 触摸屏
			if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved))
			{
				Collider terrain = MapNav.gameObject.collider;
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit hit;
				if (terrain.Raycast(ray, out hit, 1000))
				{
					var direction = hit.point - Position;
					direction.y = 0;
					Position += Vector3.Normalize(direction) * speedMainRole * Time.deltaTime;
				}
			}

			// mouse
			if (Input.GetMouseButton(0))
			{
				Collider terrain = MapNav.gameObject.collider;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (terrain.Raycast(ray, out hit, 1000))
				{
					var direction = hit.point - Position;
					direction.y = 0;
					Position += Vector3.Normalize(direction) * speedMainRole * Time.deltaTime;
				}
			}
		}
	}

	/// <summary>
	/// 更新主角信息
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	static IEnumerator Execute(MainCharacterInfo cmd)
	{
		ServerInfo = cmd;
		if (Application.loadedLevelName != "BattleScene")
		{
			yield return Application.LoadLevelAsync("BattleScene");
		}

		var my = Object.FindObjectOfType<MainCharacter>();
		my.Grid = new GridPosition() { X = (int)cmd.pos.x, Z = (int)cmd.pos.y };
	}
}
