using UnityEngine;
using System.Collections;

public class SelectTargetBoss : MonoBehaviour
{
	public UILabel nameLabel;

	internal void Present(Npc target)
	{
		nameLabel.text = target.TableInfo.name;
	}
}
