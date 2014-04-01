using UnityEngine;
using System.Collections;

public class SelectTargetNpc : MonoBehaviour
{
	public UILabel nameLabel;
	public UILabel infoLabel;

	internal void Present(Npc target)
	{
		nameLabel.text = target.TableInfo.name;
		infoLabel.text = target.TableInfo.label;
	}
}
