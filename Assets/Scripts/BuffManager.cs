using UnityEngine;
using System.Collections;
using GX.Net;
using Cmd;
using System.Collections.Generic;
using System.Linq;

public class BuffManager
{
	public static BuffManager Instance { get; private set; }
	static BuffManager() { Instance = new BuffManager(); }

	private readonly Dictionary<uint, SaveBuff> buffers = new Dictionary<uint, SaveBuff>();

	protected void OnBuffChanged()
	{
		Debug.Log(this.ToString());
	}

	public override string ToString()
	{
		return string.Format("BuffManager: {0}\n", this.buffers.Count) + 
			string.Join("\n", (from b in buffers.Values select b.ToString()).ToArray());
	}

	[Execute]
	public static void Execute(AddBuffBuffUserCmd_S cmd)
	{
		Instance.buffers.Add(cmd.buff.buffid, cmd.buff);
		Instance.OnBuffChanged();
	}
	[Execute]
	public static void Execute(AddBuffListBuffUserCmd_S cmd)
	{
		Instance.buffers.Clear();
		Instance.buffers.AddRange(from i in cmd.bufflist select new KeyValuePair<uint, SaveBuff>(i.buffid, i));
		Instance.OnBuffChanged();
	}
	[Execute]
	public static void Execute(RemoveBuffBuffUserCmd_CS cmd)
	{
		if(Instance.buffers.Remove(cmd.buffid))
			Instance.OnBuffChanged();
	}
}
