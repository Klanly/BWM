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
	public event Action<UILabel, string> UrlClicked;

	private UIWidget host;

	#region Layout
	private Vector2 m_layout;
	/// <summary>
	/// 用于当前行元素的高度调整
	/// </summary>
	private readonly List<UIWidget> m_line = new List<UIWidget>();
	/// <summary>
	/// 当前行的最大元素高度
	/// </summary>
	private float m_maxLineHeight;

	public void AddLine()
	{
		m_layout.x = 0;
		m_layout.y -= NGUIText.finalLineHeight;
		host.height = Mathf.CeilToInt(-m_layout.y);

		m_maxLineHeight = NGUIText.finalLineHeight;
		m_line.Clear();
	}
	private void Layout(UIWidget widget)
	{
		widget.MakePixelPerfect();
		widget.name = this.transform.childCount.ToString();

		// 记录行元素，用于可能发生的高度调整
		m_line.Add(widget);

		// 本行能放下，直接放
		if (widget.localSize.x <= host.width - m_layout.x)
		{
			widget.gameObject.transform.localPosition = m_layout;
		}
		else // 本行放不下，换行再放
		{
			AddLine();
			widget.gameObject.transform.localPosition = m_layout;
		}

		m_layout.x += widget.localSize.x;

		// 调整行高度
		if (widget.localSize.y > m_maxLineHeight)
		{
			var delta = widget.localSize.y - m_maxLineHeight;
			m_maxLineHeight = widget.localSize.y;
			m_layout.y -= delta;
			host.height = Mathf.CeilToInt(-m_layout.y);
			foreach (var c in m_line)
			{
				var pos = c.gameObject.transform.localPosition;
				pos.y -= delta;
				c.gameObject.transform.localPosition = pos;
			}
		}

		// 已经接近行尾则换行
		if (host.width - m_layout.x < NGUIText.finalLineHeight)
			AddLine();
		//Debug.Log(string.Format("Layot: {0}: {1}, {2}, {3}", widget.name, m_layout.y, m_layout.x, widget.localSize));
	}
	#endregion

	void Start()
	{
		host = this.GetComponent<UIWidget>();
		AddLine();
	}

	/// <summary>
	/// 添加文本，不支持NGUI的BBCode富文本编码。
	/// </summary>
	private void AddRawText(string text, string link, IList<UILabel> paragraph)
	{
		if (string.IsNullOrEmpty(text))
			return;
		text = text.Replace("\t", "    ");
		var c = CreateLabel(link);
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
			c.overflowMethod = UILabel.Overflow.ResizeFreely; // 测量结束后，恢复溢出模式
			if (string.IsNullOrEmpty(link))
				c.text = text.Substring(index, cut - index);
			else
				c.text = string.Format("[u][url={0}]{1}[/url][/u]", link, text.Substring(index, cut - index));
			paragraph.Add(c);
			Layout(c);
			if (cut >= text.Length)
				break;
			c = CreateLabel(link);
			index = cut;
		}
	}

	public IEnumerable<UILabel> AddText(string text)
	{
		return AddLink(text, null);
	}

	public IEnumerable<UILabel> AddLink(string text, string url)
	{
		if (string.IsNullOrEmpty(text))
			return Enumerable.Empty<UILabel>();
		var paragraph = new List<UILabel>();
		text = NGUIText.StripSymbols(text);
		var lines = text.Split(new char[] { '\n' });
		for (var i = 0; i < lines.Length - 1; i++)
		{
			AddRawText(lines[i], url, paragraph);
			AddLine();
		}
		AddRawText(lines.Last(), url, paragraph);
		return paragraph;
	}

	public UISprite AddSprite(string atlas, string sprite)
	{
		var res = Resources.Load<UIAtlas>(atlas);
		if (res == null)
			return null;
		var c = CreateSprite();
		c.atlas = res;
		c.spriteName = sprite;
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

	private UILabel CreateLabel(string link)
	{
		var item = NGUITools.AddChild(this.gameObject, protoLabel);
		var c = item.GetComponent<UILabel>();
		c.text = string.Empty;
		c.supportEncoding = false;
		c.overflowMethod = UILabel.Overflow.ResizeHeight;
		c.width = host.width - Mathf.CeilToInt(m_layout.x);
		c.maxLineCount = 1;
		c.rawPivot = UIWidget.Pivot.BottomLeft;
		if (string.IsNullOrEmpty(link) == false)
		{
			c.supportEncoding = true;
			var collider = c.gameObject.AddComponent<BoxCollider>();
			collider.isTrigger = true;
			UIEventListener.Get(item).onClick += go =>
			{
				if (this.UrlClicked != null)
					this.UrlClicked(c, link);
			};
		}
		return c;
	}

	private UISprite CreateSprite()
	{
		var item = NGUITools.AddChild(this.gameObject);
		var c = item.AddComponent<UISprite>();
		c.rawPivot = UIWidget.Pivot.BottomLeft;
		return c;
	}
}