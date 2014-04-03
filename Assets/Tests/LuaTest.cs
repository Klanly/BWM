using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Reflection;

public class LuaTest : MonoBehaviour
{
	string script =
@"function test(a, b)
	return a + b
end
print('Hello World! ' .. test(100, 200))";

	NLua.Lua lua;

	void Start()
	{
		lua = new NLua.Lua();
		lua.LoadCLRPackage();
		var method = typeof(Debug).GetMethod("Log", new Type[] { typeof(object) });
		lua.RegisterFunction("print", method);
	}
	void OnDestroy()
	{
		lua.Dispose();
		lua = null;
	}

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
			Debug.Log("DoString:\n" + script);
			lua.DoString(script);
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
