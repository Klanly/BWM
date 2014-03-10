using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;

public class ZoneListScene : MonoBehaviour
{
	public static ZoneInfoListLoginUserCmd_S ZoneList { get; private set; }

	public UIGrid zoneList;
	public GameObject zoneButton;

	void Start()
	{
		if (ZoneList != null)
			ShowZoneList();
	}

	private void ShowZoneList()
	{
		foreach (Transform t in zoneList.transform)
			DestroyObject(t.gameObject);
		zoneList.transform.DetachChildren();

		var height = zoneList.cellHeight * ZoneList.server.Count - (zoneList.cellHeight - zoneButton.GetComponent<UISprite>().height);
		var oldPosition = zoneList.transform.localPosition;
		oldPosition.y = height / 2.0f;
		zoneList.transform.localPosition = oldPosition;
		GameObject.Find("BG_List").GetComponent<UISprite>().height = (int)height + 70;
		zoneList.Reposition();

		foreach (var zone in ZoneList.server)
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
						gameid = ZoneList.gameid,
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
	static IEnumerator Execute(ZoneInfoListLoginUserCmd_S cmd)
	{
		ZoneList = cmd;
		yield return Application.LoadLevelAsync("ZoneListScene");
		Object.FindObjectOfType<ZoneListScene>().ShowZoneList();
	}
}
