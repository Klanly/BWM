using UnityEngine;
using System.Collections;
using System.Linq;
using Cmd;

public class RoleInfoPackage : MonoBehaviour
{
	private Transform[] items;

	void Start()
	{
		var grid = this.transform.FindChild("Grid");
		items = new Transform[grid.childCount];
		for (var i = 0; i < grid.childCount; i++)
		{
			items[i] = grid.GetChild(i);
			items[i].name = i.ToString();
		}

		Present();
	}
	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);

		Debug.Log(ItemManager.Instance.ToString());
	}

	/// <summary>
	/// Model -> View
	/// </summary>
	void Present()
	{
		foreach (var i in ItemManager.Instance.Select(ItemLocation.PackageType.Main))
		{
			items[i.loc.index].name = i.TableInfo.name;
		}
	}
}
