using UnityEngine;
using System.Collections;
using Cmd;
using System.Text;

public class ItemTooltipItem : MonoBehaviour
{
	public UILabel uiName;
	public UIButton uiClose;
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
		}
	}
	void Start()
	{
		UIEventListener.Get(uiClose.gameObject).onClick += go => this.gameObject.SetActive(false);
		UIEventListener.Get(uiDelete.gameObject).onClick += go =>
		{
			Net.Instance.Send(new RemoveItemItemUserCmd_CS() { thisid = ServerInfo.thisid });
			this.gameObject.SetActive(false);
		};
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
}
