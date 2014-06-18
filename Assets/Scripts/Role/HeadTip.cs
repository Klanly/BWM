using UnityEngine;
using System.Collections;

public class HeadTip : MonoBehaviour
{
	public UILabel text;
	public UILabel hpText;
	public UISlider hpProgress;
	public void SetHp(int curhp, int maxhp)
	{
		hpText.text = curhp.ToString()+"/" + maxhp.ToString();
		hpProgress.value = (float)curhp / maxhp;
	}
}
