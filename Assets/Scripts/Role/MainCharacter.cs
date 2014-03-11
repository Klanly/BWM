using UnityEngine;
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
	private Animator animator;
	private GameObject terrain;


	/// <summary>
	/// 主角世界坐标位置
	/// </summary>
	public Vector3 Position
	{
		get { return mainRole.position; }
		set 
		{
			if (MapNav != null)
			{
				value.x = Mathf.Clamp(value.x, 0, MapNav.gridWidth * MapNav.gridXNum);
				value.z = Mathf.Clamp(value.z, 0, MapNav.gridHeight * MapNav.gridZNum);
			}
			mainRole.position = value;

			// 设置照相机位置
			Vector3 targetCenter = mainRole.position;
			targetCenter.y += heightCameraLookAt;
			var pos = targetCenter + this.transform.rotation * Vector3.back * distanceCameraToRole;
			this.transform.position = pos;
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

	private Vector3 targetPosition;
	/// <summary>
	/// 设置目标点
	/// </summary>
	/// <value>The target position.</value>
	public Vector3 TargetPosition
	{
		get {return targetPosition;}
		set
		{
			if (MapNav != null)
			{
				value.x = Mathf.Clamp(value.x, 0, MapNav.gridWidth * MapNav.gridXNum);
				value.z = Mathf.Clamp(value.z, 0, MapNav.gridHeight * MapNav.gridZNum);
			}
			targetPosition = value;
			if(value != Vector3.zero)
			{
				if(animator.GetFloat("speed") == 0.0f)
					animator.SetFloat("speed", speedMainRole);
				
				var relativePos = TargetPosition - Position;
				mainRole.rotation = Quaternion.LookRotation(relativePos);
			}
		}
	}

	static MainCharacter()
	{
		ServerInfo = new MainCharacterInfo(); // 避免不必要的空指针判断
	}

	// Use this for initialization
	void Start()
	{
		var refRole = GameObject.Find("MainRole").transform;
		mainRole = Avatar.CreateAvatar("Prefabs/Models/Body/Sk_Female_001", "Prefabs/Models/Body/Female_Body_8100", "Prefabs/Models/Head/Female_Head_8100", "Prefabs/Models/Weapon/Weapon_Cann_1006").transform;
		mainRole.gameObject.name = "MainRole";
		mainRole.position = refRole.position;
		mainRole.rotation = refRole.rotation;
		mainRole.localScale = refRole.localScale;
		animator = mainRole.gameObject.GetComponent<Animator>();
		refRole.gameObject.SetActive(false);

		if (ServerInfo.data == null)
			return;
		LoadMap(ServerInfo.data.mapid.ToString());
		Grid = new GridPosition() { X = (int)ServerInfo.pos.x, Z = (int)ServerInfo.pos.y };
	}

	public bool LoadMap(string mapname)
	{
		var map = Resources.Load("Map/" + mapname);
		if (map == null)
		{
			Debug.LogError("Load map error: " + mapname);
			return false;
		}
		Debug.Log("Load map: " + mapname);
		if (terrain != null)
			GameObject.Destroy(terrain);
		terrain = GameObject.Instantiate(map) as GameObject;
		terrain.name = "Map." + mapname;
		MapNav = Object.FindObjectOfType<MapNav>();
		Grid = new GridPosition();
		return true;
	}

	// Update is called once per frame
	void Update()
	{
		// 设置主角的移动
		if (mainRole)
		{
			// keyboard
			float v = Input.GetAxisRaw("Vertical");
			float h = Input.GetAxisRaw("Horizontal");

			Vector3 oldPosition = Position;
			oldPosition.x += h * speedMainRole * Time.deltaTime;
			oldPosition.z += v * speedMainRole * Time.deltaTime;
			Position = oldPosition;

			// 触摸屏
			Vector3? screenPoint = null;
			if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved))
				screenPoint = Input.GetTouch(0).position;

			// mouse
			if (Input.GetMouseButton(0))
				screenPoint = Input.mousePosition;

			if (screenPoint != null)
			{
				var ray = Camera.main.ScreenPointToRay(screenPoint.Value);
				Collider terrain = MapNav.gameObject.collider;
				RaycastHit hit;
				if (terrain.Raycast(ray, out hit, 1000))
				{
					TargetPosition = hit.point;
				}
			}

			if(TargetPosition != Vector3.zero)
			{
				Vector3 vDelta = TargetPosition - Position;
				float fDeltaLen = vDelta.magnitude;
				vDelta.Normalize();

				Vector3 vOldPosition = Position;
				float fMoveLen = speedMainRole * Time.deltaTime;
				bool bFinish = false;
				if(fMoveLen >= fDeltaLen)
				{
					fMoveLen = fDeltaLen;
					bFinish = true;
				}

				Position = vOldPosition + vDelta * fMoveLen;
				if(bFinish)
				{
					TargetPosition = Vector3.zero;
					var oldRotate = mainRole.rotation;
					animator.SetFloat("speed", 0.0f);
					mainRole.rotation = oldRotate;
				}
			}
		}
	}

	/// <summary>
	/// 更新主角信息
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	static void Execute(MainCharacterInfo cmd)
	{
		ServerInfo = cmd;
		if (Application.loadedLevelName != "BattleScene")
		{
			Application.LoadLevelAsync("BattleScene");
		}
	}
}
