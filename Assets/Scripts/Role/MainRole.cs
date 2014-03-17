using UnityEngine;
using System.Collections;
using Cmd;
using GX;
using GX.Net;
using System.ComponentModel;

public class MainRole : MonoBehaviour, INotifyPropertyChanged
{
	/// <summary>
	/// 主角对应的<see cref="Role.ServerInfo"/>
	/// 主角无效时返回<see cref="MapUserData.Empty"/>而不是null，外部使用无需进行<c>null</c>判断
	/// </summary>
	public static MapUserData ServerInfo { get { return Instance != null ? Instance.Role.ServerInfo : MapUserData.Empty; } }

	#region 主角特有信息
	public uint mapid { get; set; }
	private int _maxhp;
	public int maxhp { get { return _maxhp; } set { _maxhp = value; OnPropertyChanged("maxhp"); } }
	private int _hp;
	public int hp { get { return _hp; } set { _hp = value; OnPropertyChanged("hp"); } }
	private int _maxsp;
	public int maxsp { get { return _maxsp; } set { _maxsp = value; OnPropertyChanged("maxsp"); } }
	private int _sp;
	public int sp { get { return _sp; } set { _sp = value; OnPropertyChanged("sp"); } }
	private int _exp;
	public int exp { get { return _exp; } set { _exp = value; OnPropertyChanged("exp"); } }
	public uint _level;
	public uint level { get { return _level; } set { _level = value; OnPropertyChanged("level"); } }

	#endregion


	public Role Role { get; private set; }
	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }

	public float distanceCameraToRole = 25.0f;
	public float heightCameraLookAt = 0.5f;

	private Camera cameraMain;

	private Pos lastGird = new Pos();

	/// <summary>
	/// 主角逻辑格子位置
	/// </summary>
	public Pos Grid
	{
		get { return new Pos() { x = MapNav.GetGridX(Role.Position), y = MapNav.GetGridZ(Role.Position) }; }
		set { Role.Position = MapNav.GetWorldPosition(value.x, value.y); }
	}

	private MainRole() { }

	public static MainRole Create(MapUserData info)
	{
		var role = Role.Create(info);
		role.gameObject.name = "Main." + role.gameObject.name;

		var mainRole = role.gameObject.AddComponent<MainRole>();
		mainRole.Role = role;
		role.PositionChanged += mainRole.OnPositionChanged;
		return mainRole;
	}

	public static MainRole Instance { get; private set; }
	// Use this for initialization
	void Start()
	{
		Instance = this;
		cameraMain = GameObject.Find("CameraMain").GetComponent<Camera>();
	}

	void Destory()
	{
		Instance = null;
	}

	// Update is called once per frame
	void Update()
	{
		// 设置主角的移动
		// keyboard
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		Vector3 oldPosition = Role.Position;
		oldPosition.x += h * Role.speedMainRole * Time.deltaTime;
		oldPosition.z += v * Role.speedMainRole * Time.deltaTime;
		Role.Position = oldPosition;

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
				Role.TargetPosition = hit.point;
			}
		}
	}

	void OnPositionChanged(Role sender)
	{
		UpdateCamera();
		var cur = Grid;
		if (cur != lastGird)
		{
			Net.Instance.Send(new UserMoveUpMoveUserCmd_C() { pos = cur });
			lastGird = cur;
		}
	}

	/// <summary>
	/// 根据主角位置设置照相机位置
	/// </summary>
	private void UpdateCamera()
	{
		if (cameraMain == null || MapNav == null)
			return;
		var targetCenter = this.transform.position;
		targetCenter.z += heightCameraLookAt;

		var dx = cameraMain.orthographicSize * Screen.width / Screen.height;
		var dz = cameraMain.orthographicSize / Mathf.Sin(cameraMain.transform.rotation.eulerAngles.x * Mathf.Deg2Rad) + heightCameraLookAt;

		targetCenter.x = Mathf.Clamp(targetCenter.x, dx, MapNav.transform.localScale.x - dx);
		targetCenter.z = Mathf.Clamp(targetCenter.z, dz, MapNav.transform.localScale.y - dz);

		var pos = targetCenter + cameraMain.transform.rotation * Vector3.back * distanceCameraToRole;
		cameraMain.transform.position = pos;
	}

	#region INotifyPropertyChanged Members

	public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName)
	{
		if (PropertyChanged != null)
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
	}

	#endregion

	/// <summary>
	/// 更新主角信息
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	static IEnumerator Execute(FirstMainUserDataAndPosMapUserCmd_S cmd)
	{
		if (Application.loadedLevelName != "BattleScene")
		{
			yield return Application.LoadLevelAsync("BattleScene");
		}

		BattleScene.Instance.LoadMap(table.TableMap.Select(cmd.mapid).path);
		var mainRole = MainRole.Create(cmd.data.userdata);
		mainRole.Grid = cmd.pos;
		mainRole.level = cmd.data.level;
	}

	[Execute]
	static void Execute(SetUserHpSpDataUserCmd_S cmd)
	{
		var my = MainRole.Instance;
		if (my != null && cmd.charid == my.Role.ServerInfo.charid)
		{
			my.maxhp = cmd.maxhp;
			my.hp = cmd.hp;
			my.maxsp = cmd.maxsp;
			my.sp = cmd.sp;
		}
	}

	[Execute]
	static void Execute(SetUserHpDataUserCmd_S cmd)
	{
		if (cmd.charid == MainRole.ServerInfo.charid)
		{
			MainRole.Instance.hp = cmd.curhp;
		}
	}

	[Execute]
	static void Execute(SetUserSpDataUserCmd_S cmd)
	{
		if (cmd.charid == MainRole.ServerInfo.charid)
		{
			MainRole.Instance.sp = cmd.cursp;
		}
	}
}
