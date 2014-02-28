using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd.Login;

public class ZoneListScene : MonoBehaviour
{
	public static ZoneInfoList_S ZoneInfoList { get; set; }

	public UIGrid zoneList;
	public GameObject zoneButton;

	// Use this for initialization
	void Start()
	{
		Execute(ZoneInfoList);
		ZoneInfoList = null;
	}

	void Execute(ZoneInfoList_S cmd)
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
					UIEventListener.Get(item).onClick = go => Net.Instance.Send(new UserLoginRequest_C()
					{
						gameversion = LoginScene.Version, 
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
}
