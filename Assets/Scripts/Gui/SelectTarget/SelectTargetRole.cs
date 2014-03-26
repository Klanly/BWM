using UnityEngine;
using System.Collections;

public class SelectTargetRole : MonoBehaviour
{
	public UILabel uiName;
	public UISprite uiHead;

	internal void Present(Role target)
	{
		uiName.text = target.ServerInfo.charname;
		uiHead.spriteName = target.ServerInfo.GetRoleHeadSprite();
	}
}
