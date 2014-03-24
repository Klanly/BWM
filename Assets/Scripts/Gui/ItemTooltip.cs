using UnityEngine;
using System.Collections;
using Cmd;
using System.Text;

public class ItemTooltip : MonoBehaviour
{
	public UILabel nameLabel;
	public UIButton closeButton;
	public UIButton deleteButton;

	public UILabel itemPropertyLabel;
	public UILabel itemMessageLabel;

	public UILabel equipFightLabel;
	public UILabel equipPropertyLabel;
	public UILabel equipMessageLabel;

	private SaveItem serverInfo;
	public SaveItem ServerInfo
	{
		get { return serverInfo; }
		set
		{
			serverInfo = value;
			if (serverInfo.TableInfo.type < 100)
				PresentEquip();
			else
				PresentItem();
		}
	}
	void Start()
	{
		UIEventListener.Get(closeButton.gameObject).onClick += go => this.gameObject.SetActive(false);
		UIEventListener.Get(deleteButton.gameObject).onClick += go =>
		{
			Net.Instance.Send(new RemoveItemItemUserCmd_CS() { thisid = ServerInfo.thisid });
			this.gameObject.SetActive(false);
		};
	}

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}

	void BringForward()
	{
		NGUITools.BringForward(this.closeButton.gameObject);
		NGUITools.BringForward(this.nameLabel.gameObject);
		NGUITools.BringForward(this.deleteButton.gameObject);
	}

	/// <summary>
	/// 显示道具悬浮提示
	/// </summary>
	void PresentItem()
	{
		var item = ServerInfo.TableInfo;
		this.transform.Find("Item").gameObject.SetActive(true);
		this.transform.Find("Equip").gameObject.SetActive(false);
		BringForward();

		nameLabel.text = item.name;
		itemPropertyLabel.text = string.Format("种类: {0}", item.type);
		itemMessageLabel.text = item.desc;
	}

	/// <summary>
	/// 显示装备悬浮提示
	/// </summary>
	void PresentEquip()
	{
		var item = ServerInfo.TableInfo;
		this.transform.Find("Item").gameObject.SetActive(false);
		this.transform.Find("Equip").gameObject.SetActive(true);
		BringForward();

		nameLabel.text = item.name;
		equipFightLabel.text = string.Format("战斗力{0}", item.id);
		var sb = new StringBuilder();
		sb
			.AppendFormat("种类: {0}({1})", item.type, item.Profession.GetName()).AppendLine()
			.AppendFormat("等级: [ff0000]{0}[-]", item.level);
		equipPropertyLabel.text = sb.ToString();
		equipMessageLabel.text = item.desc;
	}
}
