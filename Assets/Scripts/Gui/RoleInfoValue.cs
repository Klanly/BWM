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
	/// <summary>物攻</summary>
	public UILabel pDam;
	/// <summary>魔攻</summary>
	public UILabel mDam;
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
		var info = MainRole.ServerInfo;
		if (groupCommon.activeSelf)
		{
			hp.value = MainRole.Instance.Role.ServerInfo.hp / (float)MainRole.Instance.maxhp;
			//exp.value = info.exp / 
			pDam.text = info.pDam.ToString();
			mDam.text = info.mDam.ToString();
			pDef.text = info.pDef.ToString();
			mDef.text = info.mDef.ToString();
			pIggnore.text = info.pIggnore.ToString();
			mIggnore.text = info.mIggnore.ToString();
			moveSpeed.text = info.moveSpeed.ToString();
			attackSpeed.text = info.attackSpeed.ToString();
			hit.text = info.hit.ToString();
		}

		if (groupExtend.activeSelf)
		{
			hide.text = info.hide.ToString();
			lucky.text = info.lucky.ToString();
			force.text = info.force.ToString();
			miss.text = info.miss.ToString();
			reflect.text = info.reflect.ToString();
		}
	}
}
