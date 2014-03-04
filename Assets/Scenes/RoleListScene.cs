using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class RoleListScene : MonoBehaviour
{
	public UIGrid roleList;
	public GameObject roleItem;
	public UIButton roleCreate;

	// Use this for initialization
	void Start()
	{
		UIEventListener.Get(roleCreate.gameObject).onClick = go => Application.LoadLevel("RoleCreateScene");
	}

	private void ShowRoleList(CharactorListReturnSelectUserCmd_S cmd)
	{
		roleCreate.isEnabled = cmd.list.Count < 5;

		foreach (Transform t in roleList.transform)
			DestroyObject(t.gameObject);
		roleList.Reposition();

		foreach (var role in cmd.list)
		{
			var item = Instantiate(roleItem) as GameObject;
			item.transform.parent = roleList.transform;
			item.transform.localScale = Vector3.one;

			var info = role;
			item.transform.Find("Name").GetComponent<UILabel>().text = info.charname;
			UIEventListener.Get(item.transform.Find("Login").gameObject).onClick = go => Net.Instance.Send(new CharactorSelectSelectUserCmd_C()
			{
				charid = info.charid,
			});

			UIEventListener.Get(item.transform.Find("Delete").gameObject).onClick = go => Net.Instance.Send(new CharactorDeleteSelectUserCmd_C()
			{
				charid = info.charid,
			});
		}
		roleList.Reposition();
	}


	[Execute]
	static IEnumerator Execute(CharactorListReturnSelectUserCmd_S cmd)
	{
		if (cmd.list.Count == 0)
		{
			Application.LoadLevel("RoleCreateScene");
		}
		else
		{
			if (Application.loadedLevelName != "RoleListScene")
			{
				Application.LoadLevel("RoleListScene");
				yield return null;
			}
			Object.FindObjectOfType<RoleListScene>().ShowRoleList(cmd);
		}
	}
}
