using UnityEngine;
using System.Collections;

public class RoleInfo : MonoBehaviour
{
	public UIButton uiQuest;

	void Start()
	{
		UIEventListener.Get(uiQuest.gameObject).onClick = go => BattleScene.Instance.Gui<QuestDialog>().gameObject.SetActive(true);
	}
	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);
	}
}
