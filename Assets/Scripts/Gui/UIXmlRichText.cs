using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

/// <summary>
/// 基于XML接口的富文本控件
/// <![CDATA[
/// 用法说明：
///	<br /> 换行
///	<n>text</n> 纯文本标签
///	<p><n>text</n></p> 定义段落，段前自动插入缩进，段后自动插入换行
///	<a href="url">text</a> 超链接
/// <img atlas="atlas path" sprite="sprite name" /> Sprite图片
/// ]]>
/// </summary>
public class UIXmlRichText : UIRichText
{
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
				case "br":
					AddLine();
					break;
				case "n":
					AddText(e.Value);
					break;
				case "a":
					AddLink(e.Value, e.Attribute("href").Value);
					break;
				case "p":
					AddText("\t");
					AddXml(e.Elements());
					AddLine();
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
}
