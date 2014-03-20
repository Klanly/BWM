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
	public Entity entity;
	public Animator animator;
	public Move move;
	public CameraFollow cameraFollow;

	private Pos lastGird = new Pos();

	private MainRole() { }

	public static MainRole Create(MapUserData info)
	{
		var role = Role.Create(info);
		role.gameObject.name = "Main" + role.gameObject.name;

		var mainRole = role.gameObject.AddComponent<MainRole>();
		mainRole.Role = role;
		mainRole.entity = role.gameObject.GetComponent<Entity>();
		mainRole.animator = role.gameObject.GetComponent<Animator>();
		mainRole.move = role.gameObject.GetComponent<Move>();
		mainRole.cameraFollow = role.gameObject.AddComponent<CameraFollow>();

		mainRole.entity.PositionChanged += mainRole.OnPositionChanged;
		return mainRole;
	}

	public static MainRole Instance { get; private set; }
	// Use this for initialization
	void Start()
	{
		Instance = this;
	}

	void OnDestroy()
	{
		Instance = null;
	}

	void OnPositionChanged(Entity sender)
	{
		cameraFollow.UpdateCamera();

		var cur = entity.Grid;
		if (cur != lastGird)
		{
			Net.Instance.Send(new UserMoveUpMoveUserCmd_C() { pos = cur });
			lastGird = cur;
		}
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
		while (BattleScene.Instance == null)
			yield return new WaitForEndOfFrame();

		BattleScene.Instance.LoadMap(table.TableMap.Select(cmd.mapid).mapfile);
		var mainRole = MainRole.Create(cmd.data.userdata);
		mainRole.entity.Grid = cmd.pos;
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
