using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;

public class ZoneListScene : MonoBehaviour
{
	public UIGrid zoneList;
	public GameObject zoneButton;

	private void ShowZoneList(ZoneInfoListLoginUserCmd_S cmd)
	{
		foreach (Transform t in zoneList.transform)
			DestroyObject(t.gameObject);
		zoneList.transform.DetachChildren();

		var height = zoneList.cellHeight * cmd.server.Count - (zoneList.cellHeight - zoneButton.GetComponent<UISprite>().height);
		var oldPosition = zoneList.transform.localPosition;
		oldPosition.y = height / 2.0f;
		zoneList.transform.localPosition = oldPosition;
		GameObject.Find("BG_List").GetComponent<UISprite>().height = (int)height + 70;
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
		zoneButton.SetActive(false);
	}


	[Execute]
	static IEnumerator Execute(ZoneInfoListLoginUserCmd_S cmd)
	{
		Application.LoadLevel("ZoneListScene");
		yield return null;
		Object.FindObjectOfType<ZoneListScene>().ShowZoneList(cmd);
	}
}
