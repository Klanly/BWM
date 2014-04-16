using UnityEngine;
using System.Collections;
using System.Linq;

public class QuestTrack : MonoBehaviour
{
	public UIXmlRichText uiXmlRichText;

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
		Debug.LogError(quests);
		uiXmlRichText.gameObject.SetActive(quests.Any());
		uiXmlRichText.Clear();
		foreach (var q in quests)
		{
			uiXmlRichText.AddXml(q.Content);
		}
	}
}
