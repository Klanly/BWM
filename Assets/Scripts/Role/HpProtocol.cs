using UnityEngine;
using System.Collections;

public class HpProtocol : MonoBehaviour
{
    public virtual int hp { get; set; }
    public virtual int maxhp { get; set; }
}

public class HpNpc : HpProtocol
{
    public Npc npc;
    void Start()
    {
        npc = GetComponent<Npc>();
    }

    public override int hp
    {
        get { return npc.ServerInfo.hp; }
        set
        {
            if (value < 0)
                value = 0;
            npc.ServerInfo.hp = value;
            SelectTarget.OnUpdate(npc);

            if (value <= 0)
            {
                npc.animator.Play("Ani_Die_1");
            }
        }
    }
    public override int maxhp
    {
        get { return npc.ServerInfo.maxhp; }
        set
        {
            if (value >= 0)
                npc.ServerInfo.maxhp = value;
            SelectTarget.OnUpdate(npc);
        }
    }
}


public class HpRole : HpProtocol
{
    public Role role;
    void Start()
    {
        role = GetComponent<Role>();
    }

    public override int hp
    {
        get { return role.ServerInfo.hp; }
        set
        {
            if (value < 0)
                value = 0;
            role.ServerInfo.hp = value;

            if (role == MainRole.Instance.Role)
            {
                MainRole.Instance.OnPropertyChanged("hp");
            }
            else if (SelectTarget.Selected != null &&
                SelectTarget.Selected.entrytype == Cmd.SceneEntryType.SceneEntryType_Player &&
                SelectTarget.Selected.entryid == role.ServerInfo.charid)
            {
                var view = BattleScene.Instance.Gui<SelectTargetRole>();
                view.SetHp(value);
            }

            if (value <= 0)
            {
                role.animator.Play("Ani_Die_1");
            }
        }
    }

    public override int maxhp
    {
        get { return role.ServerInfo.maxhp; }
        set
        {
            if (value >= 0)
                role.ServerInfo.maxhp = value;

            if (SelectTarget.Selected != null &&
                SelectTarget.Selected.entrytype == Cmd.SceneEntryType.SceneEntryType_Player &&
                SelectTarget.Selected.entryid == role.ServerInfo.charid)
            {
                var view = BattleScene.Instance.Gui<SelectTargetRole>();
                view.SetHp(hp, value);
            }
        }
    }
}
