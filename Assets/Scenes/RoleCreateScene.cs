using UnityEngine;
using System.Collections;
using Cmd;

public class RoleCreateScene : MonoBehaviour
{
	public UIInput roleNameInput;
	public UIToggle maleToggle;
	public UIButton okButton;

	void Start()
	{
		UIEventListener.Get(okButton.gameObject).onClick = go => Net.Instance.Send(new CharactorCreateSelectUserCmd_C() 
		{
			charname = roleNameInput.value,
			sexman = maleToggle.value,
		});
	}

	void Update()
	{
		okButton.isEnabled = !string.IsNullOrEmpty(roleNameInput.value);
	}
}
