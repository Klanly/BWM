using UnityEngine;
using System.Collections;

public class QuestTrack : MonoBehaviour
{
	public UIXmlRichText uiXmlRichText;

	// Use this for initialization
	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		uiXmlRichText.AddXml("测试测试测试\n");
		uiXmlRichText.AddXml("测试测试测试\n");
		uiXmlRichText.AddXml("测试测试测试\n");
		uiXmlRichText.AddXml("测试测试测试\n");
		uiXmlRichText.AddXml("测试测试测试");
	}

	// Update is called once per frame
	void Update()
	{

	}
}
