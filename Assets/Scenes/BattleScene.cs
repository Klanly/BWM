using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BattleScene : MonoBehaviour
{
	public UIRoot uiRoot;
	private readonly Dictionary<string, MonoBehaviour> guiCache = new Dictionary<string, MonoBehaviour>();

	public static BattleScene Instance { get; private set; }

	/// <summary>
	/// 获取给定类型的GUI组件，会进行单件实例的缓存，以避免重复加载
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>失败返回null</returns>
	public T Gui<T>() where T : MonoBehaviour
	{
		MonoBehaviour cache;
		if (guiCache.TryGetValue(typeof(T).FullName, out cache))
			return (T)cache;

		var go = Load(typeof(T).Name);
		if (go == null)
		{
			Debug.LogError(string.Format("[GUI] Load {0} error in file {1}", typeof(T).FullName, name));
			return default(T);
		}
		var gui = go.GetComponent<T>();
		if (gui == null)
		{
			GameObject.Destroy(go);
			Debug.LogError(string.Format("[GUI] Can't find component {0} in file {1}", typeof(T).FullName, name));
			return default(T);
		}

		guiCache[typeof(T).FullName] = gui;
		return gui;
	}

	/// <summary>
	/// 根据给定的GUI名称加载，不进行任何重复加载判断
	/// </summary>
	/// <param name="name"></param>
	/// <returns>失败返回null</returns>
	public GameObject Load(string name)
	{
		if (name.StartsWith("GX"))
			name = name.Substring(2);
		var map = Resources.Load("Prefabs/Gui/" + name);
		if (map != null)
		{
			var go = GameObject.Instantiate(map) as GameObject;
			if (go != null)
			{
				go.transform.parent = uiRoot.transform;
				go.transform.localScale = Vector3.one;
				return go;
			}
		}
		return null;
	}

	IEnumerator Start()
	{
		if (Instance != null)
			throw new System.InvalidOperationException();
		Instance = this;
		yield return null;
		Gui<GXChatInput>();
		Gui<GXChatOutput>();
		Gui<GXRoleHead>();
		Load("ControlBar");

		MainRole.Create();
	}

	void Destory()
	{
		if (Instance == null)
			throw new System.InvalidOperationException();
		Instance = null;
	}
}
