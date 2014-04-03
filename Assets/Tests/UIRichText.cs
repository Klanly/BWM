using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIWidget))]
public class UIRichText : MonoBehaviour
{
	public GameObject protoLabel;

	private UIWidget host;

	void Start()
	{
		host = this.GetComponent<UIWidget>();
		host.height = 0;
	}

	public void AddParagraph(string text)
	{
		if(string.IsNullOrEmpty(text))
			return;
		var str = text.Replace("\t", "    "); // 暂不支持tab字符的显示
		var item = NGUITools.AddChild(this.gameObject, protoLabel);
		item.transform.localPosition = new Vector3(0, -host.height, 0);
		var c = item.GetComponent<UILabel>();
		c.width = host.width;

		c.text = str;
		
		//c.AssumeNaturalSize();
		//Debug.Log(string.Format("{0}, {1}", c.width, c.localSize.x));
		//if (c.width > layoutWidth)
		//{
		//	c.width = layoutWidth;
		//	c.AssumeNaturalSize();
		//}
		host.height += (int)c.localSize.y;
	}

	public void AddWidget(GameObject widget)
	{
		widget.transform.parent = this.transform;
		widget.layer = this.gameObject.layer;
	}
}

