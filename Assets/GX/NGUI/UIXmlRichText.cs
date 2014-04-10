using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System;

/// <summary>
/// 基于XML接口的富文本控件
/// <![CDATA[
///                       用法说明：
///	换行：
///		<br />
///	文字：
///		<n>text node</n>
///		simple <n>text node</n> supported
///		<n><b>bold</b> text node</n>
///		'\t' -> "    "
///		'\n' -> <br />
///	文字修饰：
///		<b>...</b> 粗体
///		<i>...</i> 斜体
///		<u>...</u> 下划线
///		<s>...</s> 删除线
///		<sup>...<sup> 上标
///		<sub>...<sub> 下标
///	节点颜色：
///	  适用于所有节点类型
///	  color嵌套时，内节点的值优先
///	  支持的颜色格式：#RGB, #RRGGBB, #RRGGBBAA, black, blue, clear, cyan, gray, green, magenta, red, white, yellow
///		<color value="颜色">...</color>
/// 段落：
///	  段前自动插入缩进，并在有必要时插入换行；段后自动插入换行；其中可以嵌套任意节点
///		<p>...</p>
///	超链接：
///		<a href="link">...</a> 
///	图片：
///		<img atlas="atlas path" sprite="sprite name" />
///		
/// TODO:
///		sub和sup因是排版后才加的修饰，会出现多余的空白，应该在排版前就予以考虑
///		p节点的嵌套"<p><p></p></p>"不同于序列"<p></p><p></p>"，应该逐级嵌套缩进
/// ]]>
/// </summary>
public class UIXmlRichText : UIRichText
{
	public void AddXml(string text)
	{
		try
		{
			Add(XDocument.Parse("<root>" + text + "</root>").Root.Nodes(), null, null);
		}
		catch (Exception ex)
		{
			Debug.LogError("Xml解析报错: " + ex.Message + "\n" + text);
		}
	}
	public void AddXml(IEnumerable<XNode> nodes)
	{
		Add(nodes, null, null);
	}
	public void AddXml(XElement e)
	{
		Add(e, null, null);
	}

	private void Add(IEnumerable<XNode> nodes, ICollection<UIWidget> paragraph, Color? color)
	{
		foreach (var n in nodes)
		{
			var e = n as XElement;
			if (e != null)
			{
				Add(e, paragraph, color);
				continue;
			}
			var t = n as XText;
			if (t != null)
			{
				Add(t.Value, paragraph, color);
				continue;
			}
		}
	}

	private void Add(XElement e, ICollection<UIWidget> paragraph, Color? color)
	{
		switch (e.Name.ToString())
		{
			case "br":
				base.AddNewLine();
				break;
			case "n":
				Add(e.Nodes(), paragraph, color);
				break;
			case "a":
				{
					var link = e.Attribute("href").Value;
					var widgets = new List<UIWidget>();
					Add(e.Nodes(), widgets, color);
					foreach (var w in widgets)
					{
						base.AttachLink(w, link);
						if (paragraph != null)
							paragraph.Add(w);
					}
				}
				break;
			case "b":
			case "i":
			case "u":
			case "s":
			case "sub":
			case "sup":
				{
					var widgets = new List<UIWidget>();
					Add(e.Nodes(), widgets, color);
					foreach (var w in widgets)
					{
						var label = w as UILabel;
						if (label != null)
						{
							label.supportEncoding = true;
							label.text = string.Format("[{0}]{1}[/{0}]", e.Name, label.text);
						}
						if (paragraph != null)
							paragraph.Add(w);
					}
				}
				break;
			case "color":
				{
					var value = e.Attribute("value").Value;
					Color c;
					if (Extensions.ParseColor(out c, value))
					{
						Add(e.Nodes(), paragraph, c);
					}
					else
					{
						Add(e.Nodes(), paragraph, color);
					}
				}
				break;
			case "p":
				if (base.IsNewLine() == false)
					base.AddNewLine();
				Add("\t", paragraph, color);
				Add(e.Nodes(), paragraph, color);
				base.AddNewLine();
				break;
			case "img":
				{
					var atlas = e.Attribute("atlas").Value;
					var sprite = e.Attribute("sprite").Value;
					if (string.IsNullOrEmpty(atlas) == false && string.IsNullOrEmpty(sprite) == false)
					{
						var w = base.AddSprite(atlas, sprite);
						if (color.HasValue)
							w.color = color.Value;
						if (paragraph != null)
							paragraph.Add(w);
						break;
					}
				}
				break;
			default:
				break;
		}
	}
	
	private void Add(string text, ICollection<UIWidget> paragraph, Color? color)
	{
		if (color.HasValue)
		{
			var widgets = new List<UIWidget>();
			base.AddText(text, widgets);
			foreach (var w in widgets)
			{
				w.color = color.Value;
				if (paragraph != null)
					paragraph.Add(w);
			}
		}
		else
		{
			base.AddText(text, paragraph);
		}
	}
}
