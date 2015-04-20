using UnityEngine;
using System.Collections;
using Cmd;
using System.Text;

public class ItemTooltipEquiped : ItemTooltipEquip
{
	public override void OnUse(GameObject sender = null)
	{
		WebSocketClient.Instance.Send(new RefreshPosItemUserCmd_CS()
		{
			thisid = ServerInfo.thisid,
			dst = new ItemLocation() { type = ItemLocation.PackageType.Main },
		});

		BattleScene.Instance.Gui<RoleInfoPackage>().CloseAllTooltips();
	}
}
