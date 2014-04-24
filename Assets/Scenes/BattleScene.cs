using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using GX;

public class BattleScene : MonoBehaviour
{
	public static BattleScene Instance { get; private set; }

	#region GUI
	/// <summary>
	/// 获取给定类型的GUI组件
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>失败返回null</returns>
	public T Gui<T>() where T : MonoBehaviour
	{
		return this.GetComponentsDescendant<T>().FirstOrDefault();
	}

	public MonoBehaviour Gui(Type guiType)
	{
		try
		{
			var method = typeof(BattleScene).GetRuntimeMethod("Gui").MakeGenericMethod(guiType);
			return method.Invoke(BattleScene.Instance, null) as MonoBehaviour;
		}
		catch { return null; }
	}

	public MonoBehaviour Gui(string guiTypeName)
	{
		try
		{
			return Gui(System.Type.GetType(guiTypeName));
		}
		catch { return null; }
	}
	#endregion

	#region Map
	public MapNav MapNav { get; private set; }

	/// <summary>
	/// 地表的唯一实例，用于避免重复加载
	/// </summary>
	private GameObject terrain;
	/// <summary>
	/// 加载指定路径的地图prefab作为地表
	/// </summary>
	/// <param name="mapname"></param>
	/// <returns>加载是否成功</returns>
	public bool LoadMap(string mapname)
	{
		var path = "Map/" + mapname;
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
		terrain.name = path.Replace('/', '.');
		MapNav = UnityEngine.Object.FindObjectOfType<MapNav>();

		// 删除地图中在编辑器中预制的Npc
		foreach(var npc in terrain.GetComponentsInChildren<NpcEditor>())
			GameObject.Destroy(npc.gameObject);
		return true;
	}
	#endregion

	void Start()
	{
		if (Instance != null)
			throw new System.InvalidOperationException();
		Instance = this;
	}

	void OnDestroy()
	{
		if (Instance == null)
			throw new System.InvalidOperationException();
		Instance = null;
		// TODO: use event driven
		Role.All.Clear();
		Npc.All.Clear();
	}
}
