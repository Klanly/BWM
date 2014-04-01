using UnityEngine;
using System.Collections;

public class SelectTargetElite : MonoBehaviour
{
	public UILabel nameLabel;
	public UISlider uiHp;

	private int maxHp;
	public void SetHp(int cur, int max = 0)
	{
		if (max != 0)
			maxHp = max;
		uiHp.value = cur / (float)maxHp;
	}

	internal void Present(Npc target)
	{
		nameLabel.text = target.TableInfo.name;
	}
}
