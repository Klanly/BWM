using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

/// <summary>
/// 基于XML接口的富文本控件
/// <![CDATA[
///                       用法说明：
///	换行：
///		<br />
///	文字：
///		<n>text node</n>
///		simple <n>text node</n> supported
///		'\t' -> "    "
///		'\n' -> <br />
///	文字修饰：
///	  TODO: sub和sup因是排版后才加的修饰，会出现多余的空白，应该在排版前就予以考虑
///		<b>...</b> 粗体
///		<i>...</i> 斜体
///		<u>...</u> 下划线
///		<s>...</s> 删除线
///		<sup>...<sup> 上标
///		<sub>...<sub> 下标
/// 段落：
///	  段前自动插入缩进，并在有必要时插入换行；段后自动插入换行；其中可以嵌套任意节点
///	  TODO: p节点的嵌套"<p><p></p></p>"不同于序列"<p></p><p></p>"，应该逐级嵌套缩进
///		<p>...</p>
///	超链接：
///	  url为空时将退化为纯文本节点
///		<a href="url">text</a> 
///	图片：
///		<img atlas="atlas path" sprite="sprite name" />
/// ]]>
/// </summary>
public class UIXmlRichText : UIRichText
{
	public void AddXml(string text)
	{
		AddXml(XDocument.Parse("<root>" + text + "</root>").Root.Nodes(), null);
	}

	public void AddXml(IEnumerable<XNode> nodes, ICollection<UILabel> paragraph = null)
	{
		foreach (var n in nodes)
		{
			var e = n as XElement;
			if (e != null)
			{
				AddXml(e, paragraph);
				continue;
			}
			var t = n as XText;
			if (t != null)
			{
				AddText(t.Value, paragraph);
				continue;
			}
		}
	}

	public void AddXml(XElement e, ICollection<UILabel> paragraph = null)
	{
		switch (e.Name.ToString())
		{
			case "br":
				AddNewLine();
				break;
			case "n":
				AddText(e.Value, paragraph);
				break;
			case "a":
				AddLink(e.Value, e.Attribute("href").Value, paragraph);
				break;
			case "b":
			case "i":
			case "u":
			case "s":
			case "sub":
			case "sup":
				var p = new List<UILabel>();
				AddXml(e.Nodes(), p);
				foreach (var n in p)
				{
					n.supportEncoding = true;
					n.text = string.Format("[{0}]{1}[/{0}]", e.Name, n.text);
					if(paragraph != null)
						paragraph.Add(n);
				}
				break;
			case "p":
				if (IsNewLine() == false)
					AddNewLine();
				AddText("\t", paragraph);
				AddXml(e.Nodes(), paragraph);
				AddNewLine();
				break;
			case "img":
				var atlas = e.Attribute("atlas").Value;
				var sprite = e.Attribute("sprite").Value;
				if (string.IsNullOrEmpty(atlas) == false && string.IsNullOrEmpty(sprite) == false)
				{
					AddSprite(atlas, sprite);
					break;
				}
				break;
			default:
				break;
		}
	}
}
