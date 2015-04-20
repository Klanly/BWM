using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Linq;
using Pmd;

/// <summary>
/// 区列表，由LoginServer下发
/// </summary>
public class ZoneListScene : MonoBehaviour
{
	public static ZoneInfoListLoginUserPmd_S ZoneList { get; private set; }

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

		var height = zoneList.cellHeight * ZoneList.zonelist.Count - (zoneList.cellHeight - zoneButton.GetComponent<UISprite>().height);
		var oldPosition = zoneList.transform.localPosition;
		oldPosition.y = height / 2.0f;
		zoneList.transform.localPosition = oldPosition;
		GameObject.Find("BG_List").GetComponent<UISprite>().height = (int)height + 70;
		zoneList.Reposition();

		foreach (var zone in ZoneList.zonelist)
		{
			var item = Instantiate(zoneButton) as GameObject;
			item.transform.parent = zoneList.transform;
			item.transform.localScale = Vector3.one;
			item.GetComponentInChildren<UILabel>().text = zone.zonename;
			switch (zone.state)
			{
				case ZoneState.Normal:
					var zoneid = zone.zoneid;
					UIEventListener.Get(item).onClick = go => ZoneSelect(zoneid);
					break;
				case ZoneState.Shutdown:
					var button = item.GetComponentInChildren<UIButton>();
					button.isEnabled = false;
					break;
				default:
					throw new System.NotImplementedException();
			}
		}
		zoneList.Reposition();
	}

	private static void ZoneSelect(uint zoneid)
	{
		if (zoneid == 0)
			return;
		WebSocketClient.Instance.Send(new UserLoginRequestLoginUserPmd_C()
		{
			gameversion = (uint)Pmd.Config.Version.Version_Game,
			gameid = ZoneList.gameid,
			zoneid = zoneid,
			mid = SystemInfo.deviceUniqueIdentifier,
		});
	}

	void Update()
	{
		// 默认选择第一个可用的区
		if (Input.GetKeyDown(KeyCode.Return))
			ZoneSelect((from z in ZoneList.zonelist where z.state == ZoneState.Normal select z.zoneid).FirstOrDefault());
	}

	[Execute]
	public static IEnumerator Execute(ZoneInfoListLoginUserPmd_S cmd)
	{
		ZoneList = cmd;
		yield return Application.LoadLevelAsync("ZoneListScene");
		Object.FindObjectOfType<ZoneListScene>().ShowZoneList();
	}
}
