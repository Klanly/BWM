using UnityEngine;
using System.Collections;
using System.Linq;
using Cmd;

public class RoleInfoPackage : MonoBehaviour
{
	private ItemView[] items;

	void Start()
	{
		var grid = this.transform.FindChild("Grid");
		items = new ItemView[grid.childCount];
		for (var i = 0; i < grid.childCount; i++)
		{
			var view = grid.GetChild(i).GetComponent<ItemView>();
			items[i] = view;
			view.gameObject.name = i.ToString("D2");
			view.ServerInfo = null;
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
			if (i.loc.index > items.Length)
				break;
			items[i.loc.index].ServerInfo = i;
		}
	}
}
