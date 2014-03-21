using UnityEngine;
using System.Collections;
using System.Linq;
using Cmd;

public class RoleInfoPackage : MonoBehaviour
{
	private ItemGrid[] items;

	void Start()
	{
		var grid = this.transform.FindChild("Grid");
		items = new ItemGrid[grid.childCount];
		for (var i = 0; i < grid.childCount; i++)
		{
			var view = grid.GetChild(i).GetComponent<ItemGrid>();
			items[i] = view;
			view.gameObject.name = i.ToString("D2");
			view.ServerInfo = null;
		}

		ItemManager.Instance.ItemChanged += Present;
		Present(ItemManager.Instance);
	}

	void OnDestroy()
	{
		ItemManager.Instance.ItemChanged -= Present;
	}


	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);

		Debug.Log(ItemManager.Instance.ToString());
	}

	/// <summary>
	/// Model -> View
	/// </summary>
	void Present(ItemManager manager)
	{
		var i = 0;
		foreach (var item in manager.Where(ItemLocation.PackageType.Main).Take(items.Length))
			items[i++].ServerInfo = item;
		for (; i < items.Length; i++)
			items[i] = null;
	}
}
