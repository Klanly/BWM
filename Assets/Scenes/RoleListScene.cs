using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 角色列表，由游戏服务器下发
/// </summary>
public class RoleListScene : MonoBehaviour
{
	public const int MaxRoleNum = 3;
	public static List<CharSelect> RoleList { get; private set; }

	public GameObject btnRole;
	public GameObject btnRoleCreate;
	public UIGrid gridRoleList;

	private string[] spriteNameProfession = { "button_zhanshi", "button_daoshi", "button_fashi" };

	// Use this for initialization
	void Start()
	{
		btnRole.SetActive(false);
		btnRoleCreate.SetActive(false);
		if (RoleList != null)
			ShowRoleList();
	}

	private void ShowRoleList()
	{
		foreach (Transform t in gridRoleList.transform)
			DestroyObject(t.gameObject);
		gridRoleList.transform.DetachChildren();

		btnRole.SetActive(true);
		btnRoleCreate.SetActive(true);

		var num = RoleList.Count;
		if (num < MaxRoleNum)
			num += 1;
		var height = gridRoleList.cellHeight * num - (gridRoleList.cellHeight - btnRole.GetComponent<UISprite>().height);
		var oldPosition = gridRoleList.transform.localPosition;
		oldPosition.y = 0;
		oldPosition.y = height / 2.0f;
		gridRoleList.transform.localPosition = oldPosition;
		gridRoleList.Reposition();

		foreach (var role in RoleList)
		{
			var item = Instantiate(btnRole) as GameObject;
			item.transform.parent = gridRoleList.transform;
			item.transform.localScale = Vector3.one;

			var info = role;
			item.transform.Find("labelName").GetComponent<UILabel>().text = info.charname;
			item.transform.Find("labelLevel").GetComponent<UILabel>().text = "LV" + info.level;
			item.transform.Find("spriteProfession").GetComponent<UISprite>().spriteName = spriteNameProfession[(int)(info.profession) - 1];
			UIEventListener.Get(item.gameObject).onClick = go => SelectRole(info.charid);
			UIEventListener.Get(item.transform.Find("btnDelete").gameObject).onClick = go =>
				Net.Instance.Send(new CharactorDeleteSelectUserCmd_C() { charid = info.charid, });
		}

		if (RoleList.Count < MaxRoleNum)
		{
			var item = Instantiate(btnRoleCreate) as GameObject;
			item.transform.parent = gridRoleList.transform;
			item.transform.localScale = Vector3.one;

			UIEventListener.Get(item.gameObject).onClick = go => Application.LoadLevel("RoleCreateScene");
		}

		btnRole.SetActive(false);
		btnRoleCreate.SetActive(false);

		gridRoleList.Reposition();
	}

	private static void SelectRole(ulong charid)
	{
		Net.Instance.Send(new CharactorSelectSelectUserCmd_C() { charid = charid, });
	}

	void Update()
	{
		// 默认选择第一个角色
		if (Input.GetKeyDown(KeyCode.Return))
			SelectRole((from r in RoleList select r.charid).FirstOrDefault());
	}


	[Execute]
	public static IEnumerator Execute(CharactorListReturnSelectUserCmd_S cmd)
	{
		RoleList = cmd.list;
		if (cmd.list.Count == 0)
		{
			yield return Application.LoadLevelAsync("RoleCreateScene");
		}
		else
		{
			if (Application.loadedLevelName != "RoleListScene")
				yield return  Application.LoadLevelAsync("RoleListScene");
			else
				Object.FindObjectOfType<RoleListScene>().ShowRoleList();
		}
	}
}
