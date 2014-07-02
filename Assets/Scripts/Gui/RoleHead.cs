using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cmd;
using GX.Net;
using System.Linq;

public class RoleHead : MonoBehaviour
{
	public UILabel myName;
	public UILabel myLevel;
	public UISlider myHp;
	public UILabel	myHpText;
	public UISprite myHead;

    public UIButton myBuff;
    public UIButton myBuffRight;
    public UIButton myBuffDown;

	IEnumerator Start()
	{
		while (MainRole.Instance == null)
			yield return new WaitForEndOfFrame();
		MainRole.Instance.PropertyChanged += OnMainRolePropertyChanged;
		OnMainRolePropertyChanged(this, null);
        BuffManager.Instance.Changed += OnBuffChanged;
        OnBuffChanged(BuffManager.Instance);
	}

	void OnDestroy()
	{
		if (MainRole.Instance != null)
			MainRole.Instance.PropertyChanged -= OnMainRolePropertyChanged;
		if (BuffManager.Instance != null)
			BuffManager.Instance.Changed -= OnBuffChanged;
	}

	void OnMainRolePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		myName.text = MainRole.ServerInfo.userdata.charname;
		myLevel.text = MainRole.ServerInfo.level.ToString();
		myHead.spriteName = MainRole.ServerInfo.userdata.GetRoleHeadSprite();
		myHp.value = MainRole.Instance.Role.ServerInfo.hp / (float)MainRole.Instance.Role.ServerInfo.maxhp;
		myHpText.text = MainRole.Instance.Role.ServerInfo.hp + "/" + MainRole.Instance.Role.ServerInfo.maxhp;
	}

    private List<UIButton> listBtnBuff = new List<UIButton>();
    private void OnBuffChanged(BuffManager quests)
    {
        // 清空按钮
        for (int i = 0; i < listBtnBuff.Count; ++i)
            Destroy(listBtnBuff[i].gameObject);
        listBtnBuff.Clear();

        myBuff.gameObject.SetActive(true);
        myBuffRight.gameObject.SetActive(true);
        myBuffDown.gameObject.SetActive(true);

        var pos = myBuff.transform.position;
        var posright = myBuffRight.transform.position;
        var posdown = myBuffDown.transform.position;

        int x = 0;
        int y = 0;
        Vector3 curpos = pos;
        foreach (var t in BuffManager.Instance.buffers)
        {
            table.TableBuff tblbuff = Table.Query<table.TableBuff>().First(i => i.id == t.buffid && i.level == t.level);

            UIButton btn = (Instantiate(myBuff.gameObject, curpos, myBuff.transform.rotation) as GameObject).GetComponent<UIButton>();
            btn.transform.parent = myBuff.transform.parent;
            btn.transform.localScale = myBuffDown.transform.localScale;
            btn.GetComponentInChildren<UISprite>().spriteName =
                btn.disabledSprite =
                btn.hoverSprite =
                btn.pressedSprite =
                btn.normalSprite =
                btn != null ? tblbuff.icon : string.Empty;
            if (t.time == 0)
                btn.GetComponentInChildren<UILabel>().text = "";
            else
                btn.GetComponentInChildren<UILabel>().text = t.lefttime.ToString();
            listBtnBuff.Add(btn);

            x++;
            if (x >= 4)
            {
                x = 0;
                y++;
            }
            curpos = pos;
            curpos.x += x * (posright.x - pos.x);
            curpos.y += y * (posdown.y - pos.y);
        }

        myBuff.gameObject.SetActive(false);
        myBuffRight.gameObject.SetActive(false);
        myBuffDown.gameObject.SetActive(false);
    }
}
