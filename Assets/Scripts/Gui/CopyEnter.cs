﻿using UnityEngine;
using System.Collections;

public class CopyEnter : MonoBehaviour
{

	public GameObject[] uiCopys;
	public GameObject uiEnter;
	private int selected = 0;

	// Use this for initialization
	void Start()
	{
		BattleScene.AddGuiToTop(gameObject);

		// 每个副本按钮点击事件
		for (int i = 0; i < uiCopys.Length; ++i)
		{
			var index = i;
			UIEventListener.Get(uiCopys[i]).onClick = go =>
			{
				Debug.Log("copy" + index);
				foreach (var t in uiCopys)
					t.GetComponent<UIButton>().enabled = true;
				uiCopys[index].GetComponent<UIButton>().enabled = false;
				selected = index;
			};
		}

		// 进入按钮
		UIEventListener.Get(uiEnter).onClick = go =>
		{
			var my = BattleScene.Instance.Gui<CopyEnter2>();
			my.gameObject.SetActive(true);
		};
	}
}
