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
	/// 界面加到最上层
	/// </summary>
	/// <returns>为界面新创建的父Panel，界面销毁时需要销毁这个父panel</returns>
	/// <param name="gogui">界面</param>
	/// <param name="bModal">If set to <c>true</c> b modal.</param>
	public static GameObject AddGuiToTop(GameObject gogui, bool bModal = true)
	{
		var root = GameObject.Find("UI Root");

		// 创建一个depth最大的panel
		var panels = root.GetComponentsInChildren<UIPanel>();
		var maxdepth = 0;
		foreach(var t in panels)
		{
			if(t.depth > maxdepth)
				maxdepth = t.depth;
		}
		maxdepth += 1;
		var gopanel = new GameObject("Panel" + maxdepth);
		var panel = gopanel.AddComponent<UIPanel>();
		panel.depth = maxdepth;
		gopanel.layer = root.layer;
		gopanel.transform.parent = root.transform;
		gopanel.transform.localScale = Vector3.one;
		gopanel.transform.position = Vector3.zero;

		// 为当前界面创建一个接收所有输入的UISprite，模拟模态窗口
		if (bModal == true)
		{
			var gosprite = new GameObject("SpriteLayer");
			var sprite = gosprite.AddComponent<UISprite>();
			sprite.SetDimensions(0,0);
			sprite.GetComponent<UIWidget>().depth = -1000;
			var col = gosprite.AddComponent<BoxCollider>();
			col.size = new Vector3(panel.width, panel.height, 0);
			gosprite.transform.parent = gogui.transform;
			gosprite.transform.position = Vector3.zero;
		}

		// 界面加入之前创建的panel
		gogui.transform.parent = gopanel.transform;
		gogui.transform.localScale = Vector3.one;
		gogui.transform.position = Vector3.zero;
		gogui.GetComponent<UIWidget>().SetAnchor(gopanel, 0, 0, 0, 0);
		NGUITools.BringForward(gogui);
		return gopanel;
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
