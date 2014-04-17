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
/// 序列帧动画：
///   fps属性可缺省，默认值为30帧
///   loop属性可缺省，默认为true
///   frames中的'/'为sprite名称之间的分隔符
///		<ani fps="30" loop="true" atlas="atlas path" frames="sprite1/sprite2/sprite3" />
///	  prefix属性可缺省，表示该atlas中所有的图片
///		<ani fps="30" loop="true" atlas="atlas path" prefix="sprite name" />
///		
/// TODO:
///		sub和sup因是排版后才加的修饰，会出现多余的空白，应该在排版前就予以考虑
///		p节点的嵌套"<p><p></p></p>"不同于序列"<p></p><p></p>"，应该逐级嵌套缩进
///		ani多帧图片大小不一致会造成排版错乱
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
				AddText(t.Value, paragraph, color);
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
				AddLink(e, paragraph, color);
				break;
			case "b":
			case "i":
			case "u":
			case "s":
			case "sub":
			case "sup":
				AddDecorateText(e, paragraph, color);
				break;
			case "color":
				AddColor(e, paragraph, color);
				break;
			case "p":
				AddParagraph(e, paragraph, color);
				break;
			case "img":
				AddImage(e, paragraph, color);
				break;
			case "ani":
				AddImage(e, paragraph, color);
				break;
			default:
				break;
		}
	}

	private void AddText(string text, ICollection<UIWidget> paragraph, Color? color)
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

	private void AddLink(XElement e, ICollection<UIWidget> paragraph, Color? color)
	{
		var link = e.AttributeValue("href");
		var widgets = new List<UIWidget>();
		Add(e.Nodes(), widgets, color);
		foreach (var w in widgets)
		{
			base.AttachLink(w, link);
			if (paragraph != null)
				paragraph.Add(w);
		}
	}

	private void AddDecorateText(XElement e, ICollection<UIWidget> paragraph, Color? color)
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

	private void AddColor(XElement e, ICollection<UIWidget> paragraph, Color? color)
	{
		var value = e.AttributeValue("value");
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

	private void AddParagraph(XElement e, ICollection<UIWidget> paragraph, Color? color)
	{
		if (base.IsNewLine() == false)
			base.AddNewLine();
		AddText("\t", paragraph, color);
		Add(e.Nodes(), paragraph, color);
		base.AddNewLine();
	}

	private void AddImage(XElement e, ICollection<UIWidget> paragraph, Color? color)
	{
		var atlas = e.AttributeValue("atlas");
		var sprite = e.AttributeValue("sprite");
		if (string.IsNullOrEmpty(atlas) == false && string.IsNullOrEmpty(sprite) == false)
		{
			var w = base.AddSprite(atlas, sprite);
			if (color.HasValue)
				w.color = color.Value;
			if (paragraph != null)
				paragraph.Add(w);
		}
	}

	private void AddAnimation(XElement e, ICollection<UIWidget> paragraph, Color? color)
	{
		var atlas = e.AttributeValue("atlas");
		if (string.IsNullOrEmpty(atlas))
			return;

		var fps = e.AttributeValue("fps").Parse(30);
		var loop = e.AttributeValue("fps").Parse(true);

		var frames = e.AttributeValue("frames");
		if (string.IsNullOrEmpty(frames) == false)
		{
			var names = frames.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (names.Length > 0)
			{
				var w = base.AddSprite(atlas);
				var c = w.gameObject.AddComponent<UISpriteGroupAnimation>();
				c.framesPerSecond = fps;
				c.loop = loop;
				c.spriteNames = names;
				if (color.HasValue)
					w.color = color.Value;
				if (paragraph != null)
					paragraph.Add(w);
			}
			return;
		}

		{
			var prefix = e.AttributeValue("prefix");
			var w = base.AddSprite(atlas);
			var c = w.gameObject.AddComponent<UISpriteAnimation>();
			c.framesPerSecond = fps;
			c.loop = loop;
			c.namePrefix = prefix;
			if (color.HasValue)
				w.color = color.Value;
			if (paragraph != null)
				paragraph.Add(w);
		}
	}
}
