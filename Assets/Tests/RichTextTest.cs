using UnityEngine;
using System.Collections;

public class RichTextTest : MonoBehaviour
{
	public RichText uiRichText;

	void Start()
	{
		//uiRichText.AddLabel("hello world");
		uiRichText.AddLink("HELLO WORLD!");
	}

	void OnGUI()
	{
	}
}
