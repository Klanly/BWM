using UnityEngine;
using System.Text;
using System.Linq;
using GX.Net;
using Cmd;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

static class ServerLog
{
	private static readonly HashSet<LogType> filter = new HashSet<LogType>();
	private static HashSet<int> distinct;
	public static string URL { get; private set; }

	static ServerLog()
	{
		Application.RegisterLogCallback(OnLogCallback);
	}

	[Execute]
	public static void Execute(ClintLogUrlLoginUserCmd_S cmd)
	{
		distinct = cmd.distinct ? new HashSet<int>() : null;
		URL = cmd.logurl;
		SetFilter(cmd.loglevel);
	}

	private static void SetFilter(string serverLogLevel)
	{
		filter.Clear();
		switch (serverLogLevel.ToLower())
		{
			case "debug":
				filter.Add(LogType.Log);
				filter.Add(LogType.Warning);
				filter.Add(LogType.Exception);
				filter.Add(LogType.Error);
				filter.Add(LogType.Assert);
				break;
			case "info":
				filter.Add(LogType.Log);
				filter.Add(LogType.Warning);
				filter.Add(LogType.Exception);
				filter.Add(LogType.Error);
				filter.Add(LogType.Assert);
				break;
			case "warning":
				filter.Add(LogType.Warning);
				filter.Add(LogType.Exception);
				filter.Add(LogType.Error);
				filter.Add(LogType.Assert);
				break;
			case "error":
				filter.Add(LogType.Exception);
				filter.Add(LogType.Error);
				filter.Add(LogType.Assert);
				break;
			default:
				break;
		}
	}

	static void OnLogCallback(string condition, string stackTrace, LogType type)
	{
		if (filter.Contains(type) == false)
			return;
		if (distinct != null)
		{
			var key = stackTrace.GetHashCode();
			if (distinct.Contains(key))
				return;
			distinct.Add(key);
		}
		Send(new Dictionary<string, string>()
		{
			{"platform", Application.platform.ToString()},
			{"type", type.ToString()},
			{"message", condition},
			{"stack", stackTrace},
		});
	}

	public static void Send(IDictionary<string, string> log)
	{
		log["mid"] = SystemInfo.deviceUniqueIdentifier;
		var json = GX.Json.Serialize(log);
		Net.Instance.StartCoroutine(Send(json));
	}

	static IEnumerator Send(string log)
	{
		var www = new WWW(URL, GX.Encoding.GetBytes(log));
		yield return www;
	}
}
