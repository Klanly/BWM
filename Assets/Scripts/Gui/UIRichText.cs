using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 富文本控件
/// </summary>
/// <remarks>
/// 在<see cref="UILabel"/>所支持的BBCode富文本协议基础上，支持多文本段落的加入。
/// 支持自定义控件的加入，但仅可用于单行布局。
/// 不支持同一行的多个子元素混合布局。
/// </remarks>
[RequireComponent(typeof(UIWidget))]
public class UIRichText : MonoBehaviour
{
	/// <summary>
	/// 所用的文本元型(prototype)
	/// </summary>
	public GameObject protoLabel;

	/// <summary>
	/// URL点击事件
	/// </summary>
	public event Action<string> UrlClicked;

	private UIWidget host;

	#region LastLabel
	private UILabel m_lastLabel;
	private UILabel LastLabel
	{
		get
		{
			if (m_lastLabel == null)
			{
				var item = NGUITools.AddChild(this.gameObject, protoLabel);
				item.transform.localPosition = new Vector3(0, -host.height, 0);
				var label = item.GetComponent<UILabel>();
				label.width = host.width;
				label.height = 0;
				label.text = string.Empty;

				// url 点击事件
				if (item.GetComponent<BoxCollider>() != null)
				{
					UIEventListener.Get(item).onClick = go =>
					{
						if (UrlClicked != null)
							UrlClicked(label.GetUrlTouch());
					};
				}
				m_lastLabel = label;
			}
			return m_lastLabel;
		}
		set
		{
			m_lastLabel = value;
		}
	}
	#endregion

	void Start()
	{
		host = this.GetComponent<UIWidget>();
		host.height = 0;
	}

	/// <summary>
	/// 添加一个文本段落，支持NGUI的BBCode富文本编码。
	/// </summary>
	/// <param name="text">为空表示添加要给空行</param>
	/// <returns></returns>
	public UILabel AddParagraph(string text = null)
	{
		var lastHeight = LastLabel.localSize.y;
		if (string.IsNullOrEmpty(text))
		{
			LastLabel.text += "\n";
		}
		else
		{
			var str = text.Replace("\t", "    "); // 暂不支持tab字符的显示
			LastLabel.text += str + "\n";
		}
		host.height += (int)(LastLabel.localSize.y - lastHeight);
		return LastLabel;
	}

	/// <summary>
	/// 添加一个<paramref name="widget"/>控件，将占有独立的一行
	/// </summary>
	/// <param name="widget"></param>
	/// <returns></returns>
	public UIWidget AddWidget(UIWidget widget)
	{
		LastLabel = null;
		widget.gameObject.layer = this.gameObject.layer;
		widget.transform.parent = this.transform;
		widget.transform.localPosition = new Vector3(0, -host.height, 0);
		host.height += (int)widget.localSize.y;
		return widget;
	}
}

