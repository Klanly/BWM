using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cmd;
using GX.Net;
using System.Linq;
using System;

public class SkillManager : IEnumerable<KeyValuePair<uint, table.TableSkill>>
{
	/// <summary>
	/// 技能施法按钮数量
	/// </summary>
	public const int FireThumbsCount = 3;
	public static SkillManager Instance { get; private set; }
	static SkillManager() { Instance = new SkillManager(); }
	private readonly Dictionary<uint, table.TableSkill> skillLevels = new Dictionary<uint, table.TableSkill>();
	private readonly Dictionary<uint, float> lastFireTime = new Dictionary<uint, float>();

	public uint BasicSkill
	{
		get { return (from s in skillLevels where s.Value != null && s.Value.IsBasic select s.Key).FirstOrDefault(); }
	}

	public event Action<SkillManager> SkillChanged;

	protected void OnSkillChanged()
	{
		if (SkillChanged != null)
			SkillChanged(this);
		Debug.Log(this.ToString());
	}

	void Clear()
	{
		this.skillLevels.Clear();
		this.lastFireTime.Clear();
		// 对所有可能有的技能占位，方便访问
		foreach (var id in table.TableSkill.Where(MainRole.ServerInfo.userdata.profession))
			this.skillLevels[id] = null;
	}

	/// <summary>
	/// 得到给定<paramref name="skillID"/>对应的技能。如果未学习，返回null
	/// </summary>
	/// <param name="skillID"></param>
	/// <returns></returns>
	public table.TableSkill GetSkill(uint skillID)
	{
		table.TableSkill s;
		return skillLevels.TryGetValue(skillID, out s) ? s : null;
	}

	/// <summary>
	/// 得到给定技能学习到的等级，未学习的返回0
	/// </summary>
	/// <param name="skillID"></param>
	/// <returns></returns>
	public uint GetLevel(uint skillID)
	{
		var s = GetSkill(skillID);
		return s != null ? s.level : 0;
	}

	/// <summary>
	/// 得到指定技能的剩余的冷却时间
	/// </summary>
	/// <param name="skillID"></param>
	/// <returns>剩余的冷却时间(秒)，&lt;=0 表示已经冷却OK</returns>
	/// <remarks>const</remarks>
	public float CoolDown(uint skillID)
	{
		float last;
		if (lastFireTime.TryGetValue(skillID, out last) == false)
			return -1;
		var skill = skillLevels[skillID];
		var now = Time.realtimeSinceStartup;
		if(now < last)
			return 0;
		return skill.cd * 0.001f - now + last;// (skill.cd / 1000) - (now - last);
	}

	#region IEnumerable<KeyValuePair<uint,TableSkill>> Members

	public IEnumerator<KeyValuePair<uint, table.TableSkill>> GetEnumerator()
	{
		return this.skillLevels.GetEnumerator();
	}

	#endregion

	#region IEnumerable Members

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	#endregion

	public override string ToString()
	{
		return string.Format("SkillManager: {0}\n", this.skillLevels.Count) +
			string.Join("\n", this.skillLevels.Select(i =>
			{
				var s = i.Value ?? table.TableSkill.First(i.Key);
				return string.Format("<color={0}>{1}:{2} {3}</color>",
					i.Value != null ? "green" : "orange",
					s.id, s.level, s.name);
			}).ToArray());
	}

	/// <summary>
	/// 上次的选择
	/// </summary>
	private static List<GameObject> lastSelects = new List<GameObject>();
	/// <summary>
	/// 根据选中目标，选中攻击的目标
	/// </summary>
	/// <returns>The selects.</returns>
	/// <param name="skill">Skill.</param>
	/// <param name="select">Select.</param>
	public static List<GameObject> GetSelects(float radius, int maxnum, GameObject select)
	{
		if (MainRole.Instance == null)
			return new List<GameObject>();

		List<GameObject> listTarget = new List<GameObject>();
		if (select != null)
		{
			listTarget.Add(select);
			maxnum = maxnum - 1;
		}

		if (maxnum == 0)
			return listTarget;

		// 收集距离满足条件的
		List<GameObject> listRaius = new List<GameObject>();
		foreach(var npc in Npc.All)
		{
			if(npc.Value.GetComponent<HpProtocol>().hp > 0 && npc.Value.gameObject != select && Vector3.Distance(npc.Value.entity.Position, MainRole.Instance.entity.Position) <= radius)
			{
				listRaius.Add(npc.Value.gameObject);
			}
		}
		
		foreach(var role in Role.All)
		{
            if (role.Value.GetComponent<HpProtocol>().hp > 0 && role.Value != MainRole.Instance.Role && role.Value.gameObject != select && Vector3.Distance(role.Value.entity.Position, MainRole.Instance.entity.Position) <= radius)
			{
				listRaius.Add(role.Value.gameObject);
			}
		}

		if (listRaius.Count == 0)
			return listTarget;

		// 若是上次的选中目标，则继续选择, 剩余的按照朝向选择
		List<GameObject> listLast = new List<GameObject>();
		List<GameObject> listRotate = new List<GameObject>();
		foreach(var go in listRaius)
		{
			if (lastSelects.Contains(go))
				listLast.Add(go);
			else
				listRotate.Add(go);
		}

		if (listLast.Count >= maxnum)
		{
			listTarget.AddRange(listLast.GetRange(0, maxnum));
		}
		else
		{
			listRotate.Sort( delegate(GameObject go1, GameObject go2) {
				var delta1 = Math.Abs(go1.transform.rotation.eulerAngles.y - MainRole.Instance.transform.rotation.eulerAngles.y);
				var delta2 = Math.Abs(go2.transform.rotation.eulerAngles.y - MainRole.Instance.transform.rotation.eulerAngles.y);
				if (delta1 < delta2)
					return -1;
				else if (delta1 < delta2)
					return 1;
				return 0;
			});

			listTarget.AddRange(listLast.GetRange(0, listLast.Count));
			listTarget.AddRange(listRotate.GetRange(0, maxnum - listLast.Count));
		}

		lastSelects = listTarget;
		return listTarget;
	}

    /// <summary>
    /// 屏幕范围内找到最近可攻击的目标
    /// </summary>
    /// <returns></returns>
    public static GameObject GetNearestTarget()
    {
        if (MainRole.Instance == null)
            return null;

        float nearest = 50.0f;
        GameObject target = null;

        // 收集距离满足条件的
        foreach (var npc in Npc.All)
        {
            if (npc.Value.GetComponent<HpProtocol>().hp > 0)
            {
                float dist = Vector3.Distance(npc.Value.entity.Position, MainRole.Instance.entity.Position);
                if (dist < nearest)
                {
                    var renderers = npc.Value.gameObject.GetComponentsInChildren<Renderer>();
                    bool visible = false;
                    foreach (var t in renderers)
                    {
                        if (t.isVisible)
                        {
                            visible = true;
                            break;
                        }
                    }

                    if (visible)
                    {
                        nearest = dist;
                        target = npc.Value.gameObject;
                    }
                }
            }
        }

        foreach (var role in Role.All)
        {
            if (role.Value.GetComponent<HpProtocol>().hp > 0 && role.Value != MainRole.Instance.Role && role.Value.renderer != null /*&& role.Value.renderer.isVisible*/)
            {
                float dist = Vector3.Distance(role.Value.entity.Position, MainRole.Instance.entity.Position);
                if (dist < nearest)
                {
                    var renderers = role.Value.gameObject.GetComponentsInChildren<Renderer>();
                    bool visible = false;
                    foreach (var t in renderers)
                    {
                        if (t.isVisible)
                        {
                            visible = true;
                            break;
                        }
                    }

                    if (visible)
                    {
                        nearest = dist;
                        target = role.Value.gameObject;
                    }
                }
            }
        }

        return target;
    }

	/// <summary>
	/// 释放给定的技能
	/// </summary>
	/// <param name="skillID"></param>
	/// <returns>是否成功释放技能</returns>
	public static bool Fire(uint skillID)
	{
		var skill = SkillManager.Instance.GetSkill(skillID);
		if (skill == null)
			return false;
		// CD检测
		if (SkillManager.Instance.CoolDown(skillID) > 0)
			return false;
		//主角检查
		if (MainRole.Instance == null)
			return false;

		// 如果有选择目标，则检查目标距离，超出范围就跑向目标后再攻击
		GameObject goSelect = null;
		if (SelectTarget.Selected != null)
		{
			var go = SelectTarget.Selected.GetGameObject();
			if (go != null && go.GetComponent<HpProtocol>().hp > 0)
			{
				goSelect = go;
				if (Vector3.Distance(goSelect.transform.position, MainRole.Instance.transform.position) > skill.radius)
				{
					MainRole.Instance.runToTarget.Target(goSelect, skill.radius, () =>
					{
						SkillManager.Fire(skillID);
					});
					return false;
				}
			}
		}

		// 收集目标
		List<GameObject> listSelect = new List<GameObject>();
		listSelect = SkillManager.GetSelects(skill.radius, skill.maxTarget, goSelect);
		if (listSelect.Count == 0) 
		{
            // 找到屏幕内最近的目标，自动跑过去
            GameObject goNearest = SkillManager.GetNearestTarget();
            if (goNearest != null)
            {
                MainRole.Instance.runToTarget.Target(goNearest, skill.radius, () =>
                {
                    SkillManager.Fire(skillID);
                });
                return false;
            }
            else 
            {
                Debug.Log("没有攻击目标!");
            }

			return false;
		}

		// 如果第一个目标不是当前目标，则发送选择目标消息
		if (goSelect == null)
		{
			SelectTarget.Select(listSelect[0]);
		}

		Debug.Log("FireSkill: " + skill);

		// 发送攻击请求
		var cmd = new RequestUseSkillUserCmd_C() { skillid = skill.id };
		foreach(var go in listSelect)
			cmd.hurts.Add(go.GetComponent<Entry>().uid);
		SkillManager.Instance.lastFireTime[skillID] = Time.realtimeSinceStartup; // 记录施法时戳
		Net.Instance.Send(cmd);
		return true;
	}


	#region 网络消息处理
	[Execute]
	public static void Execute(AddSkillListSkillUserCmd_S cmd)
	{
		SkillManager.Instance.Clear();
		foreach (var s in cmd.skilllist)
			SkillManager.Instance.skillLevels[s.skillid] = s.TableInfo;
		SkillManager.Instance.OnSkillChanged();
	}
	[Execute]
	public static void Execute(AddSkillSkillUserCmd_S cmd)
	{
		SkillManager.Instance.skillLevels[cmd.skill.skillid] = cmd.skill.TableInfo;
		SkillManager.Instance.OnSkillChanged();
	}
	[Execute]
	public static void Execute(RemoveSkillSkillUserCmd_CS cmd)
	{
		SkillManager.Instance.skillLevels.Remove(cmd.skillid);
		SkillManager.Instance.OnSkillChanged();
	}

	/// <summary>
	/// 服务器驱动的技能生效
	/// </summary>
	/// <param name="cmd"></param>
	[Execute]
	public static void Execute(ReturnUseSkillUserCmd_S cmd)
	{
		CastSkill cast = null;
		switch (cmd.owner.entrytype)
		{
			case Cmd.SceneEntryType.SceneEntryType_Npc:
				{
					var owner = Npc.All[cmd.owner.entryid];
					if (owner != null)
						cast = owner.CastSkill;
				}
				break;
			case Cmd.SceneEntryType.SceneEntryType_Player:
				{
					var owner = Role.All[cmd.owner.entryid];
					if (owner != null)
						cast = owner.CastSkill;
				}
				break;
			default:
				break;
		}

		var skill = table.TableSkill.Where(cmd.skillid, cmd.skilllevel);
		if (cast == null || skill == null)
			return;
		cast.StartSkill(skill, cmd.hurts);
	}
	#endregion
}
