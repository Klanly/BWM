using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;

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
	private Vector2 layout;

	void Start()
	{
		host = this.GetComponent<UIWidget>();
		host.height = 0;
	}

	/// <summary>
	/// 添加一个文本段落，支持NGUI的BBCode富文本编码。
	/// </summary>
	/// <param name="text">为空表示添加要给空行</param>
	public void AddParagraph(string text)
	{
		if(string.IsNullOrEmpty(text))
			return;
		var label = CreateLabel();
		var index = 0;
		while (index < text.Length)
		{
			var cut = label.WrapLine(text, index);
			label.text = text.Substring(index, cut - index);
			layout.x += label.localSize.x;
			if (host.width - layout.x < NGUIText.finalLineHeight)
			{
				layout.x = 0;
				layout.y -= NGUIText.finalLineHeight;
			}
			//Debug.Log(string.Format("{0} {1}", label.localSize, label.text));
			if (cut >= text.Length)
				break;
			label = CreateLabel();
			index = cut;
		}
	}
	public void AddXml(string text)
	{
		AddXml(XDocument.Parse("<root>" + text + "</root>").Root.Elements());
		//AddXml(XDocument.Parse(text).Elements());
	}

	public void AddXml(IEnumerable<XElement> items)
	{
		foreach (var e in items)
		{
			Debug.Log(e);
			switch (e.Name.ToString())
			{
				case "n":
					AddParagraph(e.Value);
					break;
				case "br":
					AddParagraph("\n");
					break;
				default:
					break;
			}
		}
	}

	private UILabel CreateLabel()
	{
		var item = NGUITools.AddChild(this.gameObject, protoLabel);
		item.transform.localPosition = layout;
		var label = item.GetComponent<UILabel>();
		label.supportEncoding = false;
		label.maxLineCount = 1;
		label.width = host.width - Mathf.CeilToInt(layout.x);
		return label;
	}
}