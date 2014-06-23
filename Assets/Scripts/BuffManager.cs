using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System;
using System.Collections.Generic;
using System.Linq;

public class BuffManager
{
	public static BuffManager Instance { get; private set; }
	static BuffManager() { Instance = new BuffManager(); }

	public readonly List<SaveBuff> buffers = new List<SaveBuff>();

    public event Action<BuffManager> Changed;
    protected void OnBuffChanged()
	{
        if (Changed != null)
            Changed(this);
        Debug.Log(this.ToString());
	}

	public override string ToString()
	{
		return string.Format("BuffManager: {0}\n", this.buffers.Count) + 
            string.Join("\n", buffers.Select(i => i.ToString()).ToArray());
	}

	[Execute]
	public static void Execute(AddBuffBuffUserCmd_S cmd)
	{
        var index = Instance.buffers.FindIndex(i => i.thisid == cmd.buff.thisid);
        if (index >= 0)
        {
            Instance.buffers[index] = cmd.buff;
        }
        else 
        {
            Instance.buffers.Add(cmd.buff);
        }
		Instance.OnBuffChanged();
	}
	[Execute]
	public static void Execute(AddBuffListBuffUserCmd_S cmd)
	{
		Instance.buffers.Clear();
        foreach (var t in cmd.bufflist)
            Instance.buffers.Add(t);
		Instance.OnBuffChanged();
	}
	[Execute]
	public static void Execute(RemoveBuffBuffUserCmd_CS cmd)
	{
        var index = Instance.buffers.FindIndex(i => i.thisid == cmd.thisid);
        if (index >= 0)
        { 
            Instance.buffers.RemoveAt(index);
            Instance.OnBuffChanged();
        }
	}
}
