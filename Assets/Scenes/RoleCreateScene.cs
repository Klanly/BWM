using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

public class RoleCreateScene : MonoBehaviour
{
	public UIInput roleNameInput;
	public UIToggle maleToggle;
	public UIButton okButton;

	void Start()
	{
		roleNameInput.isSelected = true; // focus it

		UIEventListener.Get(okButton.gameObject).onClick = go => Net.Instance.Send(new CheckCharNameSelectUserCmd_CS() 
		{
			charname = roleNameInput.value,
		});
	}

	void Update()
	{
		okButton.isEnabled = !string.IsNullOrEmpty(roleNameInput.value);
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
				sexman = my.maleToggle.value,
			});
		}
	}
}
