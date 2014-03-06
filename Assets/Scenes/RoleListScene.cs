using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class RoleListScene : MonoBehaviour
{
	public static int s_maxRoleNum = 3;
	private GameObject btnRole;
	private GameObject btnRoleCreate;
	private string[] spriteNameProfession = {"button_zhanshi", "button_daoshi", "button_fashi"};

	// Use this for initialization
	void Start()
	{
		btnRole = GameObject.Find("btnRole");
		btnRoleCreate = GameObject.Find("btnRoleCreate");
		btnRole.SetActive(false);
		btnRoleCreate.SetActive(false);
	}

	private void ShowRoleList(CharactorListReturnSelectUserCmd_S cmd)
	{
		var gridRoleList = GameObject.Find("gridRoleList").GetComponent<UIGrid>();
		foreach (Transform t in gridRoleList.transform)
			DestroyObject(t.gameObject);

		var num = cmd.list.Count;
		if(num < s_maxRoleNum)
			num += 1;
		var height = gridRoleList.cellHeight * num - (gridRoleList.cellHeight - btnRole.GetComponent<UISprite>().height);
		var oldPosition = gridRoleList.transform.localPosition;
		oldPosition.y = height / 2.0f;
		gridRoleList.transform.localPosition = oldPosition;
		gridRoleList.Reposition();

		foreach (var role in cmd.list)
		{
			var item = Instantiate(btnRole) as GameObject;
			item.transform.parent = gridRoleList.transform;
			item.transform.localScale = Vector3.one;

			var info = role;
			item.transform.Find("labelName").GetComponent<UILabel>().text = info.charname;
			item.transform.Find("labelLevel").GetComponent<UILabel>().text = "LV" + 10;
			item.transform.Find("spriteProfession").GetComponent<UISprite>().spriteName = spriteNameProfession[1];
			UIEventListener.Get(item.gameObject).onClick = go => Net.Instance.Send(new CharactorSelectSelectUserCmd_C()
			{
				charid = info.charid,
			});

			UIEventListener.Get(item.transform.Find("btnDelete").gameObject).onClick = go => Net.Instance.Send(new CharactorDeleteSelectUserCmd_C()
			{
				charid = info.charid,
			});
		}

		if(cmd.list.Count < s_maxRoleNum)
		{
			var item = Instantiate(btnRoleCreate) as GameObject;
			item.transform.parent = gridRoleList.transform;
			item.transform.localScale = Vector3.one;
			UIEventListener.Get(item.gameObject).onClick = go => Application.LoadLevel("RoleCreateScene");
		}

		gridRoleList.Reposition();
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
