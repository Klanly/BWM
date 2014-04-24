using UnityEngine;
using System.Collections;

public class RoleInfoValue : MonoBehaviour
{
	public GameObject buttonCommon;
	public GameObject groupCommon;
	/// <summary>血量</summary>
	public UISlider hp;
	/// <summary>经验</summary>
	public UISlider exp;
	/// <summary>攻击</summary>
	public UILabel dam;
	/// <summary>物防</summary>
	public UILabel pDef;
	/// <summary>魔防</summary>
	public UILabel mDef;
	/// <summary>物免</summary>
	public UILabel pIggnore;
	/// <summary>魔免</summary>
	public UILabel mIggnore;
	/// <summary>速度</summary>
	public UILabel moveSpeed;
	/// <summary>攻速</summary>
	public UILabel attackSpeed;
	/// <summary>命中</summary>
	public UILabel hit;

	public GameObject buttonExtend;
	public GameObject groupExtend;
	/// <summary>闪避</summary>
	public UILabel hide;
	/// <summary>致命</summary>
	public UILabel lucky;
	/// <summary>暴击</summary>
	public UILabel force;
	/// <summary>格挡</summary>
	public UILabel miss;
	/// <summary>反伤</summary>
	public UILabel reflect;

	void Start()
	{
		UIEventListener.Get(buttonCommon).onClick = go =>
		{
			groupCommon.SetActive(true);
			groupExtend.SetActive(false);
		};
		UIEventListener.Get(buttonExtend).onClick = go =>
		{
			groupCommon.SetActive(false);
			groupExtend.SetActive(true);
		};
	}

	void Update()
	{

	}
}
