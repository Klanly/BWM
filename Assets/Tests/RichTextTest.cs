using UnityEngine;
using System.Collections;

public class RichTextTest : MonoBehaviour
{
	public UIXmlRichText test;
	public UIWidget testWidget;
	public UIScrollView scrollView;

	void Start()
	{
		var t = test as UIXmlRichText;
		t.UrlClicked += (sender, url) => Debug.Log(string.Format("{0}, {1}", sender.text, url));
		t.AddXml("<color value='red'><p>正常文本<sup><b>粗体</b>上标</sup></p></color>");
		t.AddXml("<color value='#9932CC'><p>正常文本<sub><i>斜体</i>下标</sub></p></color>");
		t.AddXml("<p><b><n>粗体</n>\t<i>粗体斜体</i>\t<u>粗体下划线</u></b>\t<s>非粗体的删除线</s></p>");
		t.AddXml("simple <n>text</n> support<p><n>text</n><p><n>text</n></p></p>");
		t.AddText("A");
		t.AddText("B");
		t.AddText("C");
		t.AddText("D");
		t.AddXml("<n>\t</n><a href=\"a test\">A TEST</a><br/>");
		t.AddXml("<p><color value='black'><n>image</n>" +
			"<color value='#FF0000FF'><img atlas=\"Atlases/SkillIcon\" sprite=\"1000\"/></color>" +
			"<color value='#00FF00'><img atlas=\"Atlases/SkillIcon\" sprite=\"1001\"/></color>" +
			"<color value='#00F'><img atlas=\"Atlases/SkillIcon\" sprite=\"1002\"/></color>" +
			"</color></p>");
		t.AddXml("<n>begin1</n><br/><n>end1</n>");
		t.AddXml("<n>begin2\nend2</n>");
		t.AddSprite("Atlases/SkillIcon", "1000");
		t.AddXml("<n>begin3</n><br/><n>end3</n>");
		t.AddLink("\n\t[eeee00]党的十八大以来，习主席以实现中国梦、强军梦为核心，鲜明提出了一系列治国理政强军的重大战略思想，为全党全军全国人民团结奋斗、早日实现中华民族伟大复兴提供了思想和理论指南。对空军来说，深入学习贯彻习主席系列重要讲话精神特别是关于国防和军队建设重要论述，就是要以党在新形势下的强军目标为统领，积极适应军事斗争特别是海上方向军事斗争形势任务的发展变化，不断提高空军部队能打仗、打胜仗能力，确保有效履行肩负的使命任务。[-]", "this is a link");
		t.AddNewLine();
		t.AddText("\t[444444]着眼全局充分认清海上方向空中斗争形势任务。充分认清维护海洋权益空中行动，对空军运用指导、力量建设和综合保障等方面提出更高要求；充分认清赢得空中斗争主动，对于有效应对海上方向各种安全威胁具有重要作用；充分认清海上维权斗争的新形势，赋予空军加快从国土防空向攻防兼备转变的新内涵，以更加强烈的使命意识、忧患意识，努力完成推进空军转型建设和军事斗争准备的历史性课题。[-]");
		scrollView.ResetPosition();
	}

	void OnGUI()
	{
	}
}
