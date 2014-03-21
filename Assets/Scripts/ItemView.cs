using UnityEngine;
using System.Collections;
using Cmd;

/// <summary>
/// 道具格子的显示视图
/// </summary>
public class ItemView : MonoBehaviour
{
	public UISprite icon;
	public UILabel num;

	private SaveItem serverInfo;
	public SaveItem ServerInfo
	{
		get { return serverInfo; }
		set
		{
			serverInfo = value;
			if (value == null)
			{
				icon.gameObject.SetActive(false);
				num.gameObject.SetActive(false);
			}
			else
			{
				icon.gameObject.SetActive(true);
				icon.spriteName = value.TableInfo.icon;

				if (value.num > 1)
				{
					num.gameObject.SetActive(true);
					num.text = value.num.ToString();
				}
			}
		}
	}
}
