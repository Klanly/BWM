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

			var index = i;
			UIEventListener.Get(grid.GetChild(i).gameObject).onClick = go => OnItemGridClicked(index);
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

	private void OnItemGridClicked(int index)
	{
		var item = items[index].ServerInfo;
		if (item == null)
			return;
		var tooltip = BattleScene.Instance.Gui<ItemTooltip>();
		tooltip.gameObject.SetActive(true);
		tooltip.ServerInfo = item;		
	}
}
