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
/// 段落：段前自动插入缩进，并在有必要时插入换行；段后自动插入换行；其中可以嵌套任意节点
///		<p><n>text</n></p>
///	超链接：url为空时将退化为纯文本节点
///		<a href="url">text</a> 
///	图片：
///		<img atlas="atlas path" sprite="sprite name" />
/// ]]>
/// </summary>
public class UIXmlRichText : UIRichText
{
	public void AddXml(string text)
	{
		foreach (var c in XDocument.Parse("<root>" + text + "</root>").Root.Nodes())
		{
			Debug.Log(string.Format("{0}, {1}", c.GetType().Name, c));
		}
		AddXml(XDocument.Parse("<root>" + text + "</root>").Root.Nodes());
		//AddXml(XDocument.Parse(text).Elements());
	}

	public void AddXml(IEnumerable<XNode> nodes)
	{
		foreach (var n in nodes)
		{
			var e = n as XElement;
			if (e != null)
			{
				AddXml(e);
				continue;
			}
			var t = n as XText;
			if (t != null)
			{
				AddText(t.Value);
				continue;
			}
		}
	}

	public void AddXml(XElement e)
	{
		switch (e.Name.ToString())
		{
			case "br":
				AddNewLine();
				break;
			case "n":
				AddText(e.Value);
				break;
			case "a":
				AddLink(e.Value, e.Attribute("href").Value);
				break;
			case "p":
				if (IsNewLine() == false)
					AddNewLine();
				AddText("\t");
				AddXml(e.Nodes());
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
