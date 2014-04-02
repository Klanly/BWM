using UnityEngine;
using System.Collections;

public class RichText : MonoBehaviour
{
	public GameObject protoLabel;
	public GameObject protoLink;

	void Start()
	{
	}

	public GameObject AddLabel(string text)
	{
		var item = NGUITools.AddChild(this.gameObject, protoLabel);
		var c = item.GetComponent<UILabel>();
		c.text = text;
		c.AssumeNaturalSize();
		return item;
	}

	public GameObject AddLink(string text)
	{
		var item = NGUITools.AddChild(this.gameObject, protoLink);
		var c = item.GetComponent<UILabel>();
		c.text = text;
		c.AssumeNaturalSize();
		item.GetComponentInChildren<BoxCollider>().size = c.localSize;
		UIEventListener.Get(item.GetComponentInChildren<UIButton>().gameObject).onClick = go => Debug.Log(text);
		return item;
	}
}

