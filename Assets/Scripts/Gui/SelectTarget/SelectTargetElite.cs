using UnityEngine;
using System.Collections;

public class SelectTargetElite : MonoBehaviour
{
	public UILabel nameLabel;
	public UISlider uiHp;

	internal void OnSelect(Npc target)
	{
		nameLabel.text = target.TableInfo.name;
	}

	internal void OnUpdate(Npc target)
	{
		uiHp.value = target.ServerInfo.hp / (float)target.ServerInfo.maxhp;
	}
}
