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

	/// <summary>
	/// 界面加到Panel下，并排在最前面
	/// </summary>
	/// <param name="go">Go.</param>
	public static void AddToPanel(GameObject go)
	{
		var rootpanel = GameObject.Find("UI Root/Panel");
		go.transform.parent = rootpanel.transform;
		go.transform.localScale = Vector3.one;
		go.transform.position = Vector3.zero;
		go.GetComponent<UIWidget>().SetAnchor(rootpanel, 0, 0, 0, 0);
		NGUITools.BringForward(go);
	}

	#endregion

	#region Map
	private MapNav m_mapNav;
	public MapNav MapNav 
	{
		get { return m_mapNav; }
		private set
		{
			m_mapNav = value;
			if (MapLoaded != null)
				MapLoaded(m_mapNav);
		}
	}

	/// <summary>
	/// 地图文件加载完毕的事件
	/// </summary>
	public event Action<MapNav> MapLoaded;

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

		// 删除地图中在编辑器中预制的Npc
		foreach(var npc in terrain.GetComponentsInChildren<NpcEditor>())
			GameObject.Destroy(npc.gameObject);

		MapNav = terrain.GetComponentInChildren<MapNav>();
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
		SceneItem.All.Clear();
	}
}
