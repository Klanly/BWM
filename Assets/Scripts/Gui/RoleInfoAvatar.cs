using UnityEngine;
using System.Collections;
using Cmd;

public class RoleInfoAvatar : MonoBehaviour
{
	private GameObject avatar;
	public ItemGrid[] items;

	void Start()
	{
		for (var i = 1; i < items.Length; i++)
		{
			var index = i;
			UIEventListener.Get(items[i].gameObject).onClick = go => OnItemGridClicked(index);
		}

		// 更新事件
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

		avatar = Avatar.Create(table.TableAvatar.Select(MainRole.ServerInfo.profession, MainRole.ServerInfo.sexman));
		avatar.name = "RoleInfoAvatar.avatar";
		avatar.transform.position = new Vector3(-10, 0, 0);
		avatar.transform.Rotate(0, 180, 0);
		avatar.transform.localScale = new Vector3(5, 5, 5);
	}

	void OnDisable()
	{
		if (avatar)
		{
			GameObject.Destroy(avatar);
			avatar = null;
		}
	}

	void Present(ItemManager manager)
	{
		for (var i = 1; i < items.Length; i++)
			items[i].ServerInfo = null;
		foreach (var item in manager.Where(ItemLocation.PackageType.Equip))
			items[item.TableInfo.Type.equipPos].ServerInfo = item;
	}

	private void OnItemGridClicked(int index)
	{
		var item = items[index].ServerInfo;
		if (item == null)
			return;
		var tooltip = BattleScene.Instance.Gui<ItemTooltipEquiped>();
		tooltip.gameObject.SetActive(true);
		tooltip.ServerInfo = item;
	}
}
