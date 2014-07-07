using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minimap : MonoBehaviour
{
	public UITexture uiMapTexture;
	public UISprite uiFlagMainRole;
	public UISprite uiFlagRole;
	public UISprite[] uiFlagNpc;
	public UILabel uiMapGrid;
	public GameObject uiCopy;
	public GameObject uiStore;

	private readonly Dictionary<Entity, UISprite> flags = new Dictionary<Entity, UISprite>();

	private float m_extent = 1024;
	/// <summary>
	/// 地图可显示部分的宽度
	/// </summary>
	public float Extent
	{
		get { return m_extent; }
		set { m_extent = value; Layout = true; }
	}

	/// <summary>
	/// 地图在3D空间中的大小
	/// </summary>
	private Vector2 mapSize;
	/// <summary>
	/// 小地图可显示部分所占总地图大小的比例
	/// </summary>
	private Vector2 relativeExtent;

	/// <summary>
	/// 重新布局的脏标记
	/// </summary>
	public bool Layout { get; set; }

	private Material material;

	void Start()
	{
		// 对material clone一份，防止运行时的修改影响到源文件
		material = (Material)GameObject.Instantiate(uiMapTexture.material);
		uiMapTexture.material = material;

		Npc.All.ItemAdd += OnNpcAdd;
		Npc.All.ItemRemove += OnNpcRemove;
		Role.All.ItemAdd += OnRoleAdd;
		Role.All.ItemRemove += OnRoleRemove;
		foreach (var i in Npc.All)
			AddFlag(i.Value.GetComponent<Entity>(), GetNpcFlag(i.Value));
		foreach (var i in Role.All)
		{
			if(i.Value.ServerInfo.charid != MainRole.ServerInfo.userdata.charid)
				AddFlag(i.Value.GetComponent<Entity>(), uiFlagRole);
		}

		UIEventListener.Get(uiCopy).onClick = go =>	Instantiate(Resources.Load("Prefabs/Gui/CopyEnter"));
	}
	public void Setup()
	{
		BattleScene.Instance.MapLoaded += OnMapChanged;
		OnMapChanged(BattleScene.Instance.MapNav);
		if (MainRole.Instance != null)
			MainRole.Instance.entity.PositionChanged += OnMainRolePositionChanged;
		OnMainRolePositionChanged(MainRole.Instance.entity);
	}

	void OnDestroy()
	{
		if (MainRole.Instance != null)
			MainRole.Instance.entity.PositionChanged -= OnMainRolePositionChanged;
		if (BattleScene.Instance != null)
			BattleScene.Instance.MapLoaded -= OnMapChanged;

		Npc.All.ItemAdd -= OnNpcAdd;
		Npc.All.ItemRemove -= OnNpcRemove;
		Role.All.ItemAdd -= OnRoleAdd;
		Role.All.ItemRemove -= OnRoleRemove;
		flags.Clear();
	}

	void Update()
	{
		if (Layout && uiMapTexture.gameObject.activeSelf && MainRole.Instance != null)
		{
			Layout = false;
			
			var relativePos = new Vector2(MainRole.Instance.entity.Position.x / mapSize.x, MainRole.Instance.entity.Position.z / mapSize.y);

			// 地图位置更新
			material.mainTextureOffset = new Vector2(
				Mathf.Clamp(relativePos.x - 0.5f * relativeExtent.x, 0.0f, 1.0f - relativeExtent.x),
				Mathf.Clamp(relativePos.y - 0.5f * relativeExtent.y, 0.0f, 1.0f - relativeExtent.y));
			material.mainTextureScale = relativeExtent;

			// force update
			if (uiMapTexture.panel != null)
			{
				uiMapTexture.panel.RemoveWidget(uiMapTexture);
				uiMapTexture.panel = null;
			}

			// 主角图标位置更新
			LayoutFlag(MainRole.Instance.entity, uiFlagMainRole);

			var pos = new MapGrid(MainRole.Instance.transform.position);
			uiMapGrid.text = string.Format("{0},{1}", pos.x, pos.z);
		}

		// 主角图标显隐
		uiFlagMainRole.gameObject.SetActive(MainRole.Instance != null);
		// 主角图标旋转
		if (MainRole.Instance != null)
			uiFlagMainRole.transform.localRotation = Quaternion.Euler(0, 180, MainRole.Instance.transform.localRotation.eulerAngles.y);

		foreach (var f in flags)
			LayoutFlag(f.Key, f.Value);
	}

	private bool AddFlag(Entity entity, UISprite proto)
	{
		if (entity == null || proto == null)
			return false;
		var sprite = NGUITools.AddChild(uiMapTexture.transform.parent.gameObject, proto.gameObject).GetComponent<UISprite>();
		sprite.name = entity.name;
		flags.Add(entity, sprite);
		return true;
	}

	private bool RemoveFlag(Entity entity)
	{
		if (entity == null)
			return false;
		UISprite sprite;
		if (flags.TryGetValue(entity, out sprite) == false)
			return false;
		flags.Remove(entity);
		GameObject.Destroy(sprite.gameObject);
		return true;
	}

	private void LayoutFlag(Entity entity, UISprite flag)
	{
		var relativePos = new Vector2(entity.Position.x / mapSize.x, entity.Position.z / mapSize.y);
		var p = relativePos - material.mainTextureOffset;
		p.x /= relativeExtent.x;
		p.y /= relativeExtent.y;
		flag.gameObject.SetActive((p - new Vector2(0.5f, 0.5f)).magnitude < 0.5f);
		if (flag.gameObject.activeSelf)
		{
			flag.leftAnchor.Set(p.x, -flag.width * 0.5f);
			flag.rightAnchor.Set(p.x, flag.width * 0.5f);
			flag.topAnchor.Set(p.y, flag.height * 0.5f);
			flag.bottomAnchor.Set(p.y, -flag.height * 0.5f);
			flag.UpdateAnchors();
		}
	}

	UISprite GetNpcFlag(Npc npc)
	{
		var index = (int)npc.TableInfo.BaseType;
		if (index >= 0 && index < uiFlagNpc.Length)
			return uiFlagNpc[index];
		return null;
	}

	private void OnNpcAdd(object sender, GX.EventArgs<KeyValuePair<ulong, Npc>> args)
	{
		AddFlag(args.Data.Value.GetComponent<Entity>(), GetNpcFlag(args.Data.Value));
	}

	private void OnNpcRemove(object sender, GX.EventArgs<KeyValuePair<ulong, Npc>> args)
	{
		try
		{
			RemoveFlag(args.Data.Value.GetComponent<Entity>());
		}
		catch (MissingReferenceException) { }
	}

	private void OnRoleAdd(object sender, GX.EventArgs<KeyValuePair<ulong, Role>> args)
	{
		if (args.Data.Value.ServerInfo.charid != MainRole.ServerInfo.userdata.charid)
			AddFlag(args.Data.Value.GetComponent<Entity>(), uiFlagRole);
	}

	private void OnRoleRemove(object sender, GX.EventArgs<KeyValuePair<ulong, Role>> args)
	{
		try
		{
			RemoveFlag(args.Data.Value.GetComponent<Entity>());
		}
		catch (MissingReferenceException) { }
	}
	void OnMainRolePositionChanged(Entity sender)
	{
		Layout = true;
	}

	void OnMapChanged(MapNav map)
	{
		uiMapTexture.mainTexture = map.transform.parent.GetComponentInChildren<MapTexture>().texture;
		uiMapTexture.gameObject.SetActive(uiMapTexture.mainTexture != null);

		mapSize = new Vector2(MapGrid.Width * map.gridXNum, MapGrid.Height * map.gridZNum);
		Extent = Mathf.Min(Extent, uiMapTexture.mainTexture.width, uiMapTexture.mainTexture.height);
		relativeExtent = new Vector2(Extent / (float)uiMapTexture.mainTexture.width, Extent / (float)uiMapTexture.mainTexture.height);

		Layout = true;
	}
}
