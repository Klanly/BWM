using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

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

	#region Layout
	private Vector2 m_layout;

	public void AddLine()
	{
		m_layout.x = 0;
		m_layout.y -= NGUIText.finalLineHeight;
	}
	private void Layout(UIWidget widget)
	{
		var space = host.width - m_layout.x;
		if (widget.localSize.x <= space)
		{
			widget.gameObject.transform.localPosition = m_layout;
		}
		else
		{
			AddLine();
			widget.gameObject.transform.localPosition = m_layout;
		}

		m_layout.x += widget.localSize.x;
		if (host.width - m_layout.x < NGUIText.finalLineHeight)
			AddLine();
		Debug.Log(string.Format("Layot: {0}", m_layout));
	}
	#endregion

	void Start()
	{
		host = this.GetComponent<UIWidget>();
		host.height = 0;
	}

	/// <summary>
	/// 添加文本，不支持NGUI的BBCode富文本编码。
	/// </summary>
	private void AddRawText(string text)
	{
		if (string.IsNullOrEmpty(text))
			return;
		text = text.Replace("\t", "    ");
		var c = CreateLabel();
		var index = 0;
		while (index < text.Length)
		{
			var cut = c.WrapLine(text, index);
			if (cut == index)
			{
				AddLine();
				c.width = host.width;
				continue;
			}
			c.overflowMethod = UILabel.Overflow.ResizeFreely;
			c.text = text.Substring(index, cut - index);
			c.MakePixelPerfect();
			Layout(c);
			if (cut >= text.Length)
				break;
			c = CreateLabel();
			index = cut;
		}
	}

	public void AddText(string text)
	{
		if (string.IsNullOrEmpty(text))
			return;
		text = NGUIText.StripSymbols(text);
		var lines = text.Split(new char[] { '\n' });
		for (var i = 0; i < lines.Length - 1; i++)
		{
			AddRawText(lines[i]);
			AddLine();
		}
		AddRawText(lines.Last());
	}


	public UISprite AddSprite(string atlas, string sprite)
	{
		var res = Resources.Load<UIAtlas>(atlas);
		if (res == null)
			return null;
		var c = CreateSprite();
		c.atlas = res;
		c.spriteName = sprite;
		c.MakePixelPerfect();
		Layout(c);
		return c;
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
					AddText(e.Value);
					break;
				case "br":
					AddLine();
					break;
				default:
					break;
			}
		}
	}

	private UILabel CreateLabel()
	{
		var item = NGUITools.AddChild(this.gameObject, protoLabel);
		item.name = this.transform.childCount.ToString();
		var c = item.GetComponent<UILabel>();
		c.text = string.Empty;
		c.supportEncoding = false;
		c.overflowMethod = UILabel.Overflow.ResizeHeight;
		c.width = host.width - Mathf.CeilToInt(m_layout.x);
		c.maxLineCount = 1;
		c.rawPivot = UIWidget.Pivot.TopLeft;
		return c;
	}
	private UISprite CreateSprite()
	{
		var item = NGUITools.AddChild(this.gameObject);
		item.name = this.transform.childCount.ToString();
		var c = item.AddComponent<UISprite>();
		Debug.Log(c.rawPivot);
		c.rawPivot = UIWidget.Pivot.TopLeft;
		return c;
	}
}