using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;

public class ZoneListScene : MonoBehaviour
{
	public static ZoneInfoListLoginUserCmd_S ZoneInfoList { get; set; }

	public UIGrid zoneList;
	public GameObject zoneButton;

	// Use this for initialization
	void Start()
	{
		Net.Instance.Dispatcher.Register(this);
		Debug.Log(Net.Instance.Dispatcher);

		Execute(ZoneInfoList);
		ZoneInfoList = null;
	}

	void OnDestroy()
	{
		Net.Instance.Dispatcher.UnRegister(this);
		Debug.Log(Net.Instance.Dispatcher);
	}

	void Execute(ZoneInfoListLoginUserCmd_S cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
		foreach (Transform t in zoneList.transform)
			DestroyObject(t.gameObject);
		zoneList.Reposition();

		foreach (var zone in cmd.server)
		{
			var item = Instantiate(zoneButton) as GameObject;
			item.transform.parent = zoneList.transform;
			item.transform.localScale = Vector3.one;
			item.GetComponentInChildren<UILabel>().text = zone.zonename;
			switch (zone.state)
			{
				case ServerState.Normal:
					var zoneid = zone.zoneid;
					UIEventListener.Get(item).onClick = go => Net.Instance.Send(new UserLoginRequestLoginUserCmd_C()
					{
						gameversion = (uint)Cmd.Config.Version.Version_Game, 
						gameid = cmd.gameid,
						zoneid = zoneid,
					});
					break;
				case ServerState.Shutdown:
					var button = item.GetComponentInChildren<UIButton>();
					button.isEnabled = false;
					break;
				default:
					throw new System.NotImplementedException();
			}
		}
		zoneList.Reposition();
	}

	[Execute]
	void Execute(UserLoginReturnOkLoginUserCmd_S cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
		Net.Instance.Open(cmd.gatewayurl);
		Net.Instance.Send(new UserLoginTokenLoginUserCmd_C() { logintempid = cmd.logintempid, accountid = cmd.accountid });
	}

	[Execute]
	void Execute(CharactorListReturnSelectUserCmd_S cmd)
	{
		Debug.Log("[EXEC]" + cmd.GetType().FullName);
		Application.LoadLevel("TestScene");
	}
}
