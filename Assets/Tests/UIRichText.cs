using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIWidget))]
public class UIRichText : MonoBehaviour
{
	public GameObject protoLabel;

	private UIWidget host;

	private UILabel m_lastLabel;
	private UILabel LastLabel
	{
		get
		{
			if (m_lastLabel == null)
			{
				var item = NGUITools.AddChild(this.gameObject, protoLabel);
				item.transform.localPosition = new Vector3(0, -host.height, 0);
				m_lastLabel = item.GetComponent<UILabel>();
				m_lastLabel.width = host.width;
				m_lastLabel.height = 0;
				m_lastLabel.text = string.Empty;
			}
			return m_lastLabel;
		}
		set
		{
			m_lastLabel = value;
		}
	}

	void Start()
	{
		host = this.GetComponent<UIWidget>();
		host.height = 0;
	}

	public void AddParagraph(string text = null)
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
	}

	public void AddWidget(GameObject widget)
	{
		LastLabel = null;
		widget.transform.parent = this.transform;
		widget.layer = this.gameObject.layer;
	}
}

