using UnityEngine;
using System.Collections;
using Cmd;
using System.Text;

public class ItemTooltipEquip : MonoBehaviour
{
	public UILabel uiName;
	public UIButton uiUse;
	public UIButton uiDelete;

	public UILabel uiFight;
	public UILabel uiProperty;
	public UILabel uiMessage;

	private SaveItem serverInfo;
	public SaveItem ServerInfo
	{
		get { return serverInfo; }
		set
		{
			serverInfo = value;
			Present();
		}
	}
	void Start()
	{
		UIEventListener.Get(uiDelete.gameObject).onClick = go =>
		{
			WebSocketClient.Instance.Send(new RemoveItemItemUserCmd_CS() { thisid = ServerInfo.thisid });
			BattleScene.Instance.Gui<RoleInfoPackage>().CloseAllTooltips();
		};
		UIEventListener.Get(uiUse.gameObject).onClick = OnUse;
	}

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}

	/// <summary>
	/// 显示装备悬浮提示
	/// </summary>
	void Present()
	{
		var item = ServerInfo.TableInfo;
		uiName.text = item.name;
		uiFight.text = string.Format("战斗力{0}", item.id);
		var sb = new StringBuilder();
		sb
			.AppendFormat("种类: {0}", item.Type.name).AppendLine()
			.AppendFormat("等级: [ff0000]{0}[-]", item.level);
		uiProperty.text = sb.ToString();
		uiMessage.text = item.desc;
	}

	public virtual void OnUse(GameObject sender = null)
	{
		WebSocketClient.Instance.Send(new RefreshPosItemUserCmd_CS()
		{
			thisid = ServerInfo.thisid,
			dst = new ItemLocation() { type = ItemLocation.PackageType.Equip },
		});

		BattleScene.Instance.Gui<RoleInfoPackage>().CloseAllTooltips();
	}
}
