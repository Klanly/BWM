using UnityEngine;
using System.Collections;
using Cmd;
using GX;
using GX.Net;
using System.ComponentModel;

public class MainRole : MonoBehaviour, INotifyPropertyChanged
{
	/// <summary>
	/// 主角基本信息
	/// </summary>
	public static MainUserData ServerInfo { get; private set; }

	#region 主角特有信息
	public uint mapid { get; set; }
	private int _maxhp;
	public int maxhp { get { return _maxhp; } set { _maxhp = value; OnPropertyChanged("maxhp"); } }
	private int _maxsp;
	public int maxsp { get { return _maxsp; } set { _maxsp = value; OnPropertyChanged("maxsp"); } }
	private int _exp;
	public int exp { get { return _exp; } set { _exp = value; OnPropertyChanged("exp"); } }

	#endregion


	public Role Role { get; private set; }
	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	public Entity entity;
	public Animator animator;
	public Move move;
	public CameraFollow cameraFollow;

	private Pos lastGird = new Pos();

	private MainRole() { }

	public static MainRole Create()
	{
		var role = Role.Create(ServerInfo.userdata);
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

	/// <summary>
	/// 进入场景
	/// </summary>
	/// <param name="cmd"></param>
	/// <returns></returns>
	[Execute]
	public static IEnumerator Execute(MainUserIntoSceneMapUserCmd_S cmd)
	{
		if (Application.loadedLevelName != "BattleScene")
		{
			yield return Application.LoadLevelAsync("BattleScene");
		}
		while (BattleScene.Instance == null)
			yield return new WaitForEndOfFrame();

		BattleScene.Instance.LoadMap(table.TableMap.Where(cmd.mapid).mapfile);
		var mainRole = MainRole.Create();
		mainRole.entity.Grid = cmd.pos;
	}
}
