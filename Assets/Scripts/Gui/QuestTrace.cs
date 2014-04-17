using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// 任务追踪界面
/// </summary>
public class QuestTrace : MonoBehaviour
{
	public UIXmlRichText uiXmlRichText;
	public GameObject uiBackground;

	// Use this for initialization
	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		QuestManager.Instance.Changed += OnQuestChanged;
		OnQuestChanged(QuestManager.Instance);
	}

	void OnDestroy()
	{
		QuestManager.Instance.Changed -= OnQuestChanged;
	}

	private void OnQuestChanged(QuestManager quests)
	{
		uiXmlRichText.gameObject.SetActive(quests.Any());
		uiBackground.SetActive(uiXmlRichText.gameObject.activeSelf);

		uiXmlRichText.Clear();
		uiXmlRichText.AddXml(string.Join("\n", quests.Select(i => i.Content).ToArray()));
	}
}
