using UnityEngine;
using System.Collections;
using Cmd;
using GX;
using GX.Net;
using System.ComponentModel;
using System.Linq;

public class MainRole : MonoBehaviour, INotifyPropertyChanged
{
	private static MainUserData _serverInfo;
	/// <summary>
	/// 主角基本信息
	/// </summary>
	public static MainUserData ServerInfo
	{
		get { return _serverInfo; }
		private set
		{
			if (_serverInfo == value)
				return;
			_serverInfo = value;
			if (MainRole.Instance != null)
				MainRole.Instance.OnPropertyChanged(null);
		}
	}

	#region 主角特有信息
	public uint mapid { get; set; }
	private int _maxhp;
	public int maxhp { get { return _maxhp; } set { _maxhp = value; OnPropertyChanged("maxhp"); } }
	private int _maxsp;
	public int maxsp { get { return _maxsp; } set { _maxsp = value; OnPropertyChanged("maxsp"); } }

	#endregion


	public Role Role { get; private set; }
	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	public Entity entity;
	public Animator animator;
	public Move move;
	public CameraFollow cameraFollow;
	public PathMove pathMove;
	public ControlMove controlMove;

	private MainRole() { }

	public static MainRole Create()
	{
		var role = Role.Create(ServerInfo.userdata);
#if UNITY_EDITOR
		role.gameObject.name = "Main" + role.gameObject.name;
#endif

		var mainRole = role.gameObject.AddComponent<MainRole>();
		mainRole.Role = role;
		mainRole.entity = role.gameObject.GetComponent<Entity>();
		mainRole.animator = role.gameObject.GetComponent<Animator>();
		mainRole.move = role.gameObject.GetComponent<Move>();
		mainRole.cameraFollow = role.gameObject.AddComponent<CameraFollow>();
		mainRole.pathMove = role.gameObject.AddComponent<PathMove>();
		mainRole.controlMove = role.gameObject.AddComponent<ControlMove>();

		mainRole.entity.PositionChanged += mainRole.OnPositionChanged;
		return mainRole;
	}

	public static MainRole Instance { get; private set; }
	// Use this for initialization
	void Start()
	{
		Instance = this;
		BattleScene.Instance.Gui<Minimap>().Setup();
	}

	void OnDestroy()
	{
		Instance = null;
	}

	void OnPositionChanged(Entity sender)
	{
		cameraFollow.UpdateCamera();
	}

	#region INotifyPropertyChanged Members

	public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

	/// <summary>
	/// <see cref="MainUserData"/>所有字段名的缓存
	/// </summary>
	private static string[] MainUserDataMemberNames;
	private static string[] MapUserDataMemberNames;
	public virtual void OnPropertyChanged(string propertyName)
	{
		if (PropertyChanged == null)
			return;
		if (propertyName != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			if (propertyName == "userdata")
			{
				if (MapUserDataMemberNames == null)
					MapUserDataMemberNames = (from p in Extensions.GetProtoMemberNames(typeof(MapUserData)) select p.Name).ToArray();
				//Debug.Log(string.Join("\n", MapUserDataMemberNames));
				foreach (var p in MapUserDataMemberNames)
					PropertyChanged(this, new PropertyChangedEventArgs(p));
			}
		}
		else
		{
			if (MainUserDataMemberNames == null)
				MainUserDataMemberNames = (from p in Extensions.GetProtoMemberNames(typeof(MainUserData)) select p.Name).ToArray();
			//Debug.Log(string.Join("\n", MainUserDataMemberNames));
			foreach (var p in MainUserDataMemberNames)
				PropertyChanged(this, new PropertyChangedEventArgs(p));
		}
	}

	#endregion

	public CastSkill CastSkill { get { return this.Role.CastSkill; } }

	/// <summary>
	/// 设置主角信息
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static void Execute(InitMainUserDataMapUserCmd_S cmd)
	{
		MainRole.ServerInfo = cmd.data;
	}

	[Execute]
	public static void Execute(MainUserDataPropertyUserCmd_S cmd)
	{
		MainRole.ServerInfo = cmd.data;
	}

	[Execute]
	public static void Execute(AddExpPropertyUserCmd_S cmd)
	{
		MainRole.ServerInfo.exp = cmd.curexp;
		MainRole.Instance.OnPropertyChanged("exp");
		// TODO: 用 cmd.type; cmd.addexp; cmd.extexp; 实现经验值增长动画
	}

	/// <summary>
	/// 进入场景
	/// </summary>
	/// <param name="cmd"></param>
	/// <returns></returns>
	[Execute]
	public static IEnumerator Execute(MainUserIntoSceneMapUserCmd_S cmd)
	{
		if (Application.loadedLevelName != "LoadingScene")
		{
			LoadingScene.loadedLevelName = "BattleScene";
			yield return Application.LoadLevelAsync("LoadingScene");
		}
		while (BattleScene.Instance == null)
			yield return new WaitForEndOfFrame();
		BattleScene.Instance.LoadMap(table.TableMap.Where(cmd.mapid).mapfile);
	}

	/// <summary>
	/// 主角升级
	/// </summary>
	/// <param name="cmd"></param>
	/// <returns></returns>
	public static bool Execute(UserLevelUpMapUserCmd_S cmd)
	{
		if(cmd.charid == ServerInfo.userdata.charid)
		{
			ServerInfo.level = cmd.level;
			MainRole.Instance.OnPropertyChanged("level");
			return true;
		}
		return false;
	}

	public void SetHp(int hp, int maxhp = -1)
	{
		if (hp < 0)
			hp = 0;
		if (maxhp >= 0)
			Role.ServerInfo.maxhp = maxhp;
		Role.ServerInfo.hp = hp;
		OnPropertyChanged("hp");

		if (hp <= 0)
		{
			animator.Play("Ani_Die_1");
		}
	}

	public void SetSp(int sp, int maxsp = -1)
	{
		if (sp < 0)
			sp = 0;
		if (maxsp >= 0)
			ServerInfo.maxsp = maxsp;
		ServerInfo.sp = sp;
		OnPropertyChanged("sp");
	}

	public static bool Execute(ChangeUserHpDataUserCmd_S cmd)
	{
		if (MainRole.ServerInfo != null && cmd.charid == MainRole.ServerInfo.userdata.charid)
		{
			MainRole.Instance.SetHp(cmd.curhp);
			return true;
		}
		return false;
	}

	public static bool Execute(SetUserHpSpDataUserCmd_S cmd)
	{
		var my = MainRole.Instance;
		if (my != null && cmd.charid == my.Role.ServerInfo.charid)
		{
			MainRole.Instance.SetHp(cmd.hp, cmd.maxhp);
			MainRole.Instance.SetSp(cmd.sp, cmd.maxsp);
			return true;
		}
		return false;
	}

	public static bool Execute(ChangeUserSpDataUserCmd_S cmd)
	{
		if (MainRole.ServerInfo != null && cmd.charid == MainRole.ServerInfo.userdata.charid)
		{
			MainRole.Instance.SetSp(cmd.cursp);
			return true;
		}
		return false;
	}
}
