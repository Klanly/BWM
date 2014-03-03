using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class RoleListScene : MonoBehaviour
{
	public UIGrid roleList;
	public GameObject roleButton;

	private void ShowRoleList(CharactorListReturnSelectUserCmd_S cmd)
	{
		foreach (Transform t in roleList.transform)
			DestroyObject(t.gameObject);
		roleList.Reposition();

		foreach (var role in cmd.list)
		{
			var item = Instantiate(roleButton) as GameObject;
			item.transform.parent = roleList.transform;
			item.transform.localScale = Vector3.one;
			item.GetComponentInChildren<UILabel>().text = role.charname;

			var info = role;
			UIEventListener.Get(item).onClick = go => Net.Instance.Send(new CharactorSelectSelectUserCmd_C()
			{
				charid = info.charid,
			});
		}
		roleList.Reposition();
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

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
			Application.LoadLevel("RoleListScene");
			yield return null;
			Object.FindObjectOfType<RoleListScene>().ShowRoleList(cmd);
		}
	}
}
