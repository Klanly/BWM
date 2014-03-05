using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class RoleCreateScene : MonoBehaviour
{
	public UIInput roleNameInput;
	public bool sexman = true;
	public int profession = 1;

	private GameObject spriteZhanshi, spriteDaoshi, spriteFashi;

	void btnMale_onClick(GameObject sender)
	{
		sexman = true;
		GameObject.Find("btnMale").GetComponent<TweenScale>().PlayForward();
		GameObject.Find("btnFemale").GetComponent<TweenScale>().PlayReverse();
	}

	void btnFemale_onClick(GameObject sender)
	{
		sexman = false;
		GameObject.Find("btnFemale").GetComponent<TweenScale>().PlayForward();
		GameObject.Find("btnMale").GetComponent<TweenScale>().PlayReverse();
	}

	void btnZhanshi_onClick(GameObject sender)
	{
		profession = 1;
		spriteZhanshi.SetActive(true);
		spriteDaoshi.SetActive(false);
		spriteFashi.SetActive(false);
		GameObject.Find("wiDesc").GetComponent<TweenColor>().ResetToBeginning();
		GameObject.Find("wiDesc").GetComponent<TweenPosition>().ResetToBeginning();
		GameObject.Find("wiDesc").GetComponent<TweenColor>().PlayForward();
		GameObject.Find("wiDesc").GetComponent<TweenPosition>().PlayForward();
	}

	void btnDaoshi_onClick(GameObject sender)
	{
		profession = 2;
		spriteZhanshi.SetActive(false);
		spriteDaoshi.SetActive(true);
		spriteFashi.SetActive(false);
		GameObject.Find("wiDesc").GetComponent<TweenColor>().ResetToBeginning();
		GameObject.Find("wiDesc").GetComponent<TweenPosition>().ResetToBeginning();
		GameObject.Find("wiDesc").GetComponent<TweenColor>().PlayForward();
		GameObject.Find("wiDesc").GetComponent<TweenPosition>().PlayForward();
	}

	void btnFashi_onClick(GameObject sender)
	{
		profession = 3;
		spriteZhanshi.SetActive(false);
		spriteDaoshi.SetActive(false);
		spriteFashi.SetActive(true);
		GameObject.Find("wiDesc").GetComponent<TweenColor>().ResetToBeginning();
		GameObject.Find("wiDesc").GetComponent<TweenPosition>().ResetToBeginning();
		GameObject.Find("wiDesc").GetComponent<TweenColor>().PlayForward();
		GameObject.Find("wiDesc").GetComponent<TweenPosition>().PlayForward();
	}

	void btnSuiji_onClick(GameObject sender)
	{
		var t = Table.Query<table.TableNameItem>();
		var name = t.Random().name1 + t.Random().name2 + t.Random().name3 + t.Random().name4;
		roleNameInput.value = name;
	}

	void btnOK_onClick(GameObject sender)
	{
		Net.Instance.Send(new CheckCharNameSelectUserCmd_CS()
        {
			charname = roleNameInput.value,
		});
	}

	void Start()
	{
		UIEventListener.Get(GameObject.Find("btnMale")).onClick = this.btnMale_onClick;
		UIEventListener.Get(GameObject.Find("btnFemale")).onClick = this.btnFemale_onClick;
		UIEventListener.Get(GameObject.Find("btnZhanshi")).onClick = this.btnZhanshi_onClick;
		UIEventListener.Get(GameObject.Find("btnDaoshi")).onClick = this.btnDaoshi_onClick;
		UIEventListener.Get(GameObject.Find("btnFashi")).onClick = this.btnFashi_onClick;
		UIEventListener.Get(GameObject.Find("btnSuiji")).onClick = this.btnSuiji_onClick;
		UIEventListener.Get(GameObject.Find("btnOK")).onClick = this.btnOK_onClick;

		spriteZhanshi = GameObject.Find("spriteZhanshi");
		spriteDaoshi = GameObject.Find("spriteDaoshi");
		spriteFashi = GameObject.Find("spriteFashi");
		spriteZhanshi.SetActive(false);
		spriteDaoshi.SetActive(false);
		spriteFashi.SetActive(false);

		// 随机玩家名
		roleNameInput = GameObject.Find("inputRoleName").GetComponent<UIInput>();
		btnSuiji_onClick(null);

		// 初始选择战士、女性
		btnZhanshi_onClick(null);
		btnFemale_onClick(null);
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
				//profession = my.profession;
			});
		}
	}
}
