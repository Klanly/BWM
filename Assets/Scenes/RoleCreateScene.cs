using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;
using System.Collections.Generic;

public class RoleCreateScene : MonoBehaviour
{
	public UIInput roleNameInput;
	public bool sexman = true;
	public Profession profession = default(Profession);
	private readonly Dictionary<Profession, GameObject> professionSprites = new Dictionary<Profession, GameObject>();
	private readonly Dictionary<Profession, GameObject> professionButtons = new Dictionary<Profession, GameObject>();

	private string[] strSk = new string[]{"Prefabs/Models/Body/Sk_Female", "Prefabs/Models/Body/Sk_Female"};
	private string[] strFemaleBody = new string[]{"Prefabs/Models/Body/Female_Body_8100", "Prefabs/Models/Body/Female_Body_8100","Prefabs/Models/Body/Female_Body_8100"};
	private string[] strFemaleHead = new string[]{"Prefabs/Models/Head/Female_Head_8100", "Prefabs/Models/Head/Female_Head_8100","Prefabs/Models/Head/Female_Head_8100"};
	private string[] strFemaleWeapon = new string[]{"Prefabs/Models/Weapon/Weapon_Cann_1006", "Prefabs/Models/Weapon/Weapon_Cann_1006","Prefabs/Models/Weapon/Weapon_Cann_1006"};
	private string[] strFemaleAction = new string[]{"Ani_Dd_show", "Ani_Dd_show","Ani_Dd_show"};
	private string[] strMaleBody = new string[]{"Prefabs/Models/Body/Female_Body_8100", "Prefabs/Models/Body/Female_Body_8100","Prefabs/Models/Body/Female_Body_8100"};
	private string[] strMaleHead = new string[]{"Prefabs/Models/Head/Female_Head_8100", "Prefabs/Models/Head/Female_Head_8100","Prefabs/Models/Head/Female_Head_8100"};
	private string[] strMaleWeapon = new string[]{"Prefabs/Models/Weapon/Weapon_Cann_1006", "Prefabs/Models/Weapon/Weapon_Cann_1006","Prefabs/Models/Weapon/Weapon_Cann_1006"};
	private string[] strMaleAction = new string[]{"Ani_Dd_show", "Ani_Dd_show","Ani_Dd_show"};

	private GameObject spriteZhanshi, spriteDaoshi, spriteFashi;
	private GameObject avatar;
	private GameObject curAvatar = null;

	void btnMale_onClick(GameObject sender)
	{
		sexman = true;
		GameObject.Find("btnMale").GetComponent<TweenScale>().PlayForward();
		GameObject.Find("btnFemale").GetComponent<TweenScale>().PlayReverse();
		CreateAvatar();
	}

	void btnFemale_onClick(GameObject sender)
	{
		sexman = false;
		GameObject.Find("btnFemale").GetComponent<TweenScale>().PlayForward();
		GameObject.Find("btnMale").GetComponent<TweenScale>().PlayReverse();
		CreateAvatar();
	}

	void CreateAvatar()
	{
		if(curAvatar != null)
			Destroy(curAvatar);

		string sk, body, head, weapon, action;
		int index = (int)profession - 1;
		if(sexman == true)
		{
			sk = strSk[0];
			body = strMaleBody[index];
			head = strMaleHead[index];
			weapon = strMaleWeapon[index];
			action = strMaleAction[index];
		}
		else
		{
			sk = strSk[1];
			body = strFemaleBody[index];
			head = strFemaleHead[index];
			weapon = strFemaleWeapon[index];
			action = strFemaleAction[index];
		}

		var go = Avatar.CreateAvatar(sk, body, head, weapon);
		go.transform.position = avatar.transform.position;
		go.transform.rotation = avatar.transform.rotation;
		go.transform.localScale = avatar.transform.localScale;
		go.GetComponent<Animator>().Play(action);
		curAvatar = go;
	}

	void OnProfessionClick(Profession p)
	{
		profession = p;
		foreach (var b in professionSprites)
			b.Value.SetActive(b.Key == profession);
		GameObject.Find("wiDesc").GetComponent<TweenColor>().ResetToBeginning();
		GameObject.Find("wiDesc").GetComponent<TweenPosition>().ResetToBeginning();
		GameObject.Find("wiDesc").GetComponent<TweenColor>().PlayForward();
		GameObject.Find("wiDesc").GetComponent<TweenPosition>().PlayForward();
		CreateAvatar();
	}

	void btnSuiji_onClick(GameObject sender)
	{
		var t = Table.Query<table.TableNameItem>();
		var name = t.Random().name1 + t.Random().name2 + t.Random().name3 + t.Random().name4;
		roleNameInput.value = name;
	}

	void btnOK_onClick(GameObject sender)
	{
		if (profession == default(Profession) || string.IsNullOrEmpty(roleNameInput.value))
			return;
		Net.Instance.Send(new CheckCharNameSelectUserCmd_CS()
		{
			charname = roleNameInput.value,
		});
	}

	void Start()
	{
		var quit = this.gameObject.GetComponent<EscapeQuit>();
		quit.lastSceneName = RoleListScene.RoleList.Count == 0 ? "ZoneListScene" : "RoleListScene";

		UIEventListener.Get(GameObject.Find("btnMale")).onClick = this.btnMale_onClick;
		UIEventListener.Get(GameObject.Find("btnFemale")).onClick = this.btnFemale_onClick;
		UIEventListener.Get(GameObject.Find("btnSuiji")).onClick = this.btnSuiji_onClick;
		UIEventListener.Get(GameObject.Find("btnOK")).onClick = this.btnOK_onClick;

		professionButtons[Profession.Profession_ZhanShi] = GameObject.Find("btnZhanshi");
		professionButtons[Profession.Profession_DaoShi] = GameObject.Find("btnDaoshi");
		professionButtons[Profession.Profession_FaShi] = GameObject.Find("btnFashi");
		professionSprites[Profession.Profession_ZhanShi] = GameObject.Find("spriteZhanshi");
		professionSprites[Profession.Profession_DaoShi] = GameObject.Find("spriteDaoshi");
		professionSprites[Profession.Profession_FaShi] = GameObject.Find("spriteFashi");
		foreach (var p in professionButtons)
		{
			var item = p;
			UIEventListener.Get(item.Value).onClick = s => OnProfessionClick(item.Key);
		}

		// 找到avatar
		avatar = GameObject.Find("Avatar");
		avatar.SetActive(false);

		// 随机玩家名
		roleNameInput = GameObject.Find("inputRoleName").GetComponent<UIInput>();
		btnSuiji_onClick(null);

		// 初始选择战士、女性
		OnProfessionClick(Profession.Profession_ZhanShi);
		btnFemale_onClick(null);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
			btnOK_onClick(this.gameObject);
	}

	[Execute]
	static void Execute(CheckCharNameSelectUserCmd_CS cmd)
	{
		var my = Object.FindObjectOfType<RoleCreateScene>();
		if (cmd.exist)
		{
			my.roleNameInput.value = string.Empty;
			my.roleNameInput.isSelected = true;
		}
		else
		{
			Net.Instance.Send(new CharactorCreateSelectUserCmd_C()
			{
				charname = cmd.charname,
				sexman = my.sexman,
				profession = my.profession,
			});
		}
	}
}
