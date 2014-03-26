using UnityEngine;
using System.Collections;

public class SelectTargetElite : MonoBehaviour
{
	public UILabel nameLabel;

	internal void Present(Npc target)
	{
		nameLabel.text = target.TableInfo.name;
	}
}
