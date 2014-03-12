using UnityEngine;
using System.Collections;
using Cmd;
using GX;
using GX.Net;
using MainRoleInfo = Cmd.FirstMainUserDataAndPosMapUserCmd_S;

public class MainRole : MonoBehaviour
{
	public static MainRoleInfo ServerInfo { get; private set; }

	public float speedMainRole = 5.0f;
	public float distanceCameraToRole = 25.0f;
	public float heightCameraLookAt = 0.5f;

	public MapNav MapNav { get; private set; }

	private Animator animator;
	private GameObject terrain;
	private Camera camera;


	/// <summary>
	/// 主角世界坐标位置
	/// </summary>
	public Vector3 Position
	{
		get { return this.transform.position; }
		set
		{
			if (MapNav != null)
			{
				value.x = Mathf.Clamp(value.x, 0, MapNav.gridWidth * MapNav.gridXNum);
				value.z = Mathf.Clamp(value.z, 0, MapNav.gridHeight * MapNav.gridZNum);
			}
			this.transform.position = value;
			UpdateCamera();
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
		get { return targetPosition; }
		set
		{
			if (MapNav != null)
			{
				value.x = Mathf.Clamp(value.x, 0, MapNav.gridWidth * MapNav.gridXNum);
				value.z = Mathf.Clamp(value.z, 0, MapNav.gridHeight * MapNav.gridZNum);
			}
			targetPosition = value;
			if (value != Vector3.zero)
			{
				if (animator.GetFloat("speed") == 0.0f)
					animator.SetFloat("speed", speedMainRole);

				var relativePos = TargetPosition - Position;
				this.transform.rotation = Quaternion.LookRotation(relativePos);
			}
		}
	}

	static MainRole()
	{
		ServerInfo = new MainRoleInfo(); // 避免不必要的空指针判断
	}

	// Use this for initialization
	void Start()
	{
		if (ServerInfo.data == null)
			return;
		camera = Object.FindObjectOfType<Camera>();
		LoadMap(ServerInfo.data.mapid.ToString());
		Grid = new GridPosition() { X = (int)ServerInfo.pos.x, Z = (int)ServerInfo.pos.y };
		UpdateCamera();
	}

	public static MainRole Create()
	{
		var avatar = Avatar.CreateAvatar("Prefabs/Models/Body/Sk_Female_001", "Prefabs/Models/Body/Female_Body_8100", "Prefabs/Models/Head/Female_Head_8100", "Prefabs/Models/Weapon/Weapon_Cann_1006");
		avatar.name = "MainRole";
		avatar.transform.localScale = new Vector3(5, 5, 5);
		var role = avatar.AddComponent<MainRole>();
		role.animator = avatar.GetComponent<Animator>();
		return role;
	}

	/// <summary>
	/// 加载指定路径的地图prefab作为地表
	/// </summary>
	/// <param name="mapname"></param>
	/// <returns>加载是否成功</returns>
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

		if (TargetPosition != Vector3.zero)
		{
			Vector3 vDelta = TargetPosition - Position;
			float fDeltaLen = vDelta.magnitude;
			vDelta.Normalize();

			Vector3 vOldPosition = Position;
			float fMoveLen = speedMainRole * Time.deltaTime;
			bool bFinish = false;
			if (fMoveLen >= fDeltaLen)
			{
				fMoveLen = fDeltaLen;
				bFinish = true;
			}

			Position = vOldPosition + vDelta * fMoveLen;
			if (bFinish)
			{
				TargetPosition = Vector3.zero;
				var oldRotate = this.transform.rotation;
				animator.SetFloat("speed", 0.0f);
				this.transform.rotation = oldRotate;
			}
		}
	}

	/// <summary>
	/// 根据主角位置设置照相机位置
	/// </summary>
	private void UpdateCamera()
	{
		var targetCenter = this.transform.position;
		targetCenter.y += heightCameraLookAt;
		var pos = targetCenter + camera.transform.rotation * Vector3.back * distanceCameraToRole;
		camera.transform.position = pos;
	}

	/// <summary>
	/// 更新主角信息
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	static void Execute(MainRoleInfo cmd)
	{
		ServerInfo = cmd;
		if (Application.loadedLevelName != "BattleScene")
		{
			Application.LoadLevelAsync("BattleScene");
		}
	}
}
