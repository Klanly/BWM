﻿using UnityEngine;
using System.Collections;
using Cmd;
using System.Text;

public class ItemTooltipEquip : MonoBehaviour
{
	public UILabel uiName;
	public UIButton uiClose;
	public UIButton uiSetup;
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
}
