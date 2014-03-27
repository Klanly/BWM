using UnityEngine;
using System.Collections;
using Cmd;
using System.Text;

public class ItemTooltipEquiped : ItemTooltipEquip
{
	public override void OnUse(GameObject sender = null)
	{
		// TODO: 应发送服务器请求指令，以下为本地临时测试代码
		var setup = ServerInfo.DeepClone();
		setup.loc.type = ItemLocation.PackageType.Main;
		Net.Instance.SendToMe(new AddItemItemUserCmd_S() { item = setup });

		BattleScene.Instance.Gui<RoleInfoPackage>().CloseAllTooltips();
	}
}
