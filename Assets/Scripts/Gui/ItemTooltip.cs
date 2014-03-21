using UnityEngine;
using System.Collections;
using Cmd;

public class ItemTooltip : MonoBehaviour
{
	public UILabel nameLabel;
	public UILabel messageLabel;
	public UIButton closeButton;
	public UIButton deleteButton;

	private SaveItem serverInfo;
	public SaveItem ServerInfo
	{
		get { return serverInfo; }
		set
		{
			serverInfo = value;
			nameLabel.text = value.TableInfo.name;
			messageLabel.text = value.TableInfo.desc;
		}
	}
	void Start()
	{
		UIEventListener.Get(closeButton.gameObject).onClick += go => this.gameObject.SetActive(false);
		UIEventListener.Get(deleteButton.gameObject).onClick += go => this.gameObject.SetActive(false);
	}

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}
}
