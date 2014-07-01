using UnityEngine;
using System.Collections;
using Cmd;
using System.Text;

public class ItemTooltipItem : MonoBehaviour
{
	public UILabel uiName;
	public UIButton uiUse;
	public UIButton uiDelete;

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
			uiUse.gameObject.SetActive(serverInfo.TableInfo.Type.CanUse);
		}
	}
	void Start()
	{
		UIEventListener.Get(uiDelete.gameObject).onClick = go =>
		{
			Net.Instance.Send(new RemoveItemItemUserCmd_CS() { thisid = ServerInfo.thisid });
			BattleScene.Instance.Gui<RoleInfoPackage>().CloseAllTooltips();
		};
		UIEventListener.Get(uiUse.gameObject).onClick = OnUse;
	}

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}


	/// <summary>
	/// 显示道具悬浮提示
	/// </summary>
	void Present()
	{
		var item = ServerInfo.TableInfo;
		uiName.text = item.name;
		uiProperty.text = string.Format("种类: {0}", item.type);
		uiMessage.text = item.desc;
	}

	public virtual void OnUse(GameObject sender = null)
	{
		Net.Instance.Send(new UseItemItemUserCmd_CS()
		{
			thisid = ServerInfo.thisid,
			targetid = 0,
		});

		BattleScene.Instance.Gui<RoleInfoPackage>().CloseAllTooltips();
	}
}
