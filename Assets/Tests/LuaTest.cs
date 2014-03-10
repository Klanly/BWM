using UnityEngine;
using System.Collections;
using System;

public class LuaTest : MonoBehaviour
{
	string script = string.Empty;

	void OnGUI()
	{
		script = GUI.TextArea(new Rect(10, 10, Screen.width - 20, Screen.height - 50), script);
		if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height - 30, 80, 20), "OK"))
		{
			Run();
		}
	}

	private bool Run()
	{
		try
		{
			Debug.Log("Run... \n" + script);
			using (var lua = new NLua.Lua())
			{
				lua.LoadCLRPackage();
				var returns = lua.DoString(script);
				Debug.Log(returns);
			}
			Debug.Log("OK");
			return true;
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			return false;
		}
	}
}
