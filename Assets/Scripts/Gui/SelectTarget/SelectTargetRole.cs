using UnityEngine;
using System.Collections;

public class SelectTargetRole : MonoBehaviour
{
	public UILabel uiName;
	public UILabel uiLevel;
	public UISprite uiHead;
	public UISlider uiHp;
	public UILabel uiHpText;

	private int maxHp;
	private int maxSp;

	public void SetHp(int cur, int max = 0)
	{
		if (max != 0)
			maxHp = max;
		uiHp.value = cur / (float)maxHp;
		uiHpText.text = cur + "/" + maxHp;
	}

	public void SetSp(int cur, int max = 0)
	{
		if (max != 0)
			maxSp = max;
	}

	internal void OnSelect(Role target)
	{
		uiName.text = target.ServerInfo.charname;
		uiHead.spriteName = target.ServerInfo.GetRoleHeadSprite();
		uiLevel.text = target.ServerInfo.level.ToString();
	}
}
