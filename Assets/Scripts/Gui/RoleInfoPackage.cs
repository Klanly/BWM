using UnityEngine;
using System.Collections;
using System.Linq;
using Cmd;

public class RoleInfoPackage : MonoBehaviour
{
	public UIButton tidyButton;
	private ItemGrid[] items;

	void Start()
	{
		// 包裹整理
		UIEventListener.Get(tidyButton.gameObject).onClick = go =>
			Net.Instance.Send(new TidyItemItemUserCmd_C());

		// 道具格子表初始化
		var grid = this.transform.FindChild("Grid");
		items = new ItemGrid[grid.childCount];
		for (var i = 0; i < grid.childCount; i++)
		{
			var view = grid.GetChild(i).GetComponent<ItemGrid>();
			items[i] = view;
			view.gameObject.name = i.ToString("D2");
			view.ServerInfo = null;

			// 道具格子点击
			var index = i;
			UIEventListener.Get(grid.GetChild(i).gameObject).onClick = go => OnItemGridClicked(index);
		}

		// 道具格子表更新事件
		ItemManager.Instance.ItemChanged += Present;
		Present(ItemManager.Instance);
	}

	void OnDestroy()
	{
		ItemManager.Instance.ItemChanged -= Present;
	}


	void OnEnable()
	{
		Present(ItemManager.Instance);
	}

	/// <summary>
	/// Model -> View
	/// </summary>
	void Present(ItemManager manager)
	{
		if (this.gameObject.activeSelf == false || items == null)
			return;
		var i = 0;
		foreach (var item in manager.Where(ItemLocation.PackageType.Main).Take(items.Length))
			items[i++].ServerInfo = item;
		for (; i < items.Length; i++)
			items[i].ServerInfo = null;
	}

	private void OnItemGridClicked(int index)
	{
		var item = items[index].ServerInfo;
		if (item == null)
			return;
		if (item.TableInfo.Type.IsEquip)
		{
			var tooltip = BattleScene.Instance.Gui<ItemTooltipEquip>();
			tooltip.gameObject.SetActive(true);
			tooltip.ServerInfo = item;

			var equiped = (
				from i in ItemManager.Instance.Where(ItemLocation.PackageType.Equip)
				where i.TableInfo.Type.equipPos == item.TableInfo.Type.equipPos
				select i).FirstOrDefault();
			if (equiped != null)
			{
				var compare = BattleScene.Instance.Gui<ItemTooltipEquiped>();
				compare.gameObject.SetActive(true);
				compare.ServerInfo = equiped;
			}
		}
		else
		{
			var tooltip = BattleScene.Instance.Gui<ItemTooltipItem>();
			tooltip.gameObject.SetActive(true);
			tooltip.ServerInfo = item;
		}
	}

	public void CloseAllTooltips()
	{
		BattleScene.Instance.Gui<ItemTooltipEquip>().gameObject.SetActive(false);
		BattleScene.Instance.Gui<ItemTooltipEquiped>().gameObject.SetActive(false);
		BattleScene.Instance.Gui<ItemTooltipItem>().gameObject.SetActive(false);
	}
}
