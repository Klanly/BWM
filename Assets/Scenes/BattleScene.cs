using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BattleScene : MonoBehaviour
{
	public static BattleScene Instance { get; private set; }

	#region GUI
	public UIRoot uiRoot;
	private readonly Dictionary<string, MonoBehaviour> guiCache = new Dictionary<string, MonoBehaviour>();

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

		var go = LoadGui(typeof(T).Name);
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
	public GameObject LoadGui(string name)
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
				go.transform.position = Vector3.zero;
				return go;
			}
		}
		return null;
	}
	#endregion

	#region Map
	/// <summary>
	/// 地表的唯一实例，用于避免重复加载
	/// </summary>
	private GameObject terrain;
	/// <summary>
	/// 加载指定路径的地图prefab作为地表
	/// </summary>
	/// <param name="path"></param>
	/// <returns>加载是否成功</returns>
	public bool LoadMap(string path)
	{
		var map = Resources.Load(path);
		if (map == null)
		{
			Debug.LogError("Load map error: " + path);
			return false;
		}
		Debug.Log("Load map: " + path);
		if (terrain != null)
			GameObject.Destroy(terrain);
		terrain = GameObject.Instantiate(map) as GameObject;
		terrain.name = path;
		return true;
	}
	#endregion

	IEnumerator Start()
	{
		if (Instance != null)
			throw new System.InvalidOperationException();
		Instance = this;
		yield return null;
		Gui<GXChatInput>();
		Gui<GXChatOutput>();
		Gui<GXRoleHead>();
		LoadGui("ControlBar");

		LoadMap(table.TableMapItem.Select(MainRole.ServerInfo.data.mapid).path);

		MainRole.Create();
	}

	void Destory()
	{
		if (Instance == null)
			throw new System.InvalidOperationException();
		Instance = null;
	}
}
