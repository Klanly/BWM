using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(MapNav))]
public class MapNavEditor : Editor
{
	#region SerializedProperty
	/// <summary>
	/// 格子宽(米)
	/// </summary>
	private SerializedProperty gridWidth;
	/// <summary>
	/// X轴格子数
	/// </summary>
	private SerializedProperty gridXNum;
	/// <summary>
	/// Z轴格子数
	/// </summary>
	private SerializedProperty gridZNum;
	#endregion

	/// <summary>
	/// 当前修改的阻挡标志
	/// </summary>
	private MapNav.TileType curTileType = MapNav.TileType.Walk;
	/// <summary>
	/// 当前操作类型
	/// </summary>
	private ProcessType curProcessType = ProcessType.None;
	/// <summary>
	/// 操作直径
	/// </summary>
	private int radius = 1;

	public MapNav Target { get { return (MapNav)target; } }

	/// <summary>
	/// 操作类型
	/// </summary>
	public enum ProcessType
	{
		/// <summary>
		/// 没有操作
		/// </summary>
		None = 0,
		/// <summary>
		/// 设置
		/// </summary>
		Set = 1,
		/// <summary>
		/// 清空
		/// </summary>
		Clear = 2,
		/// <summary>
		/// 寻路
		/// </summary>
		FindPath = 3,
	}

	/// <summary>
	/// 自动寻路起始点
	/// </summary>
	private Vector3 vecStart = new Vector3(0, 0, 0);
	/// <summary>
	/// 自动寻路结束点
	/// </summary>
	private Vector3 vecEnd = new Vector3(0, 0, 0);
	/// <summary>
	/// 自动寻路路径
	/// </summary>
	private List<Cmd.Pos> path = new List<Cmd.Pos>();
	/// <summary>
	/// 是否采样寻路的起始点
	/// </summary>
	private bool bSampleStart = true;

	/// <summary>
	/// The folds.
	/// </summary>
	private static bool[] folds = { false, true, true, false, true, true, false, false };


	static MapNavEditor()
	{
		ProtoBuf.Serializer.PrepareSerializer<MapNav>();
	}

	void OnEnable()
	{
		gridWidth = serializedObject.FindProperty("gridWidth");
		gridXNum = serializedObject.FindProperty("gridXNum");
		gridZNum = serializedObject.FindProperty("gridZNum");
	}

	public override void OnInspectorGUI()
	{
		Tools.current = Tool.View;

		EditorGUILayout.Space();
		serializedObject.Update();

		GUI.changed = false;

		// 创建格子
		folds[0] = EditorGUILayout.BeginToggleGroup("创建格子", folds[0]);
		if (folds[0])
		{
			GUILayout.Label("注意这会删除当前格子", EditorStyles.label);
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(gridWidth, new GUIContent("格子宽(米)"));
			EditorGUILayout.PropertyField(gridXNum, new GUIContent("X轴格子数"));
			EditorGUILayout.PropertyField(gridZNum, new GUIContent("Z轴格子数"));
			if (GUILayout.Button("Update"))
			{
				Target.Reset();
			}
		}
		EditorGUILayout.EndToggleGroup();

		// 刷格子
		GUILayout.Space(20f);
		folds[1] = EditorGUILayout.BeginToggleGroup("刷阻挡", folds[1]);
		if (folds[1])
		{
			EditorGUILayout.Space();

			Target.ShowGrids = EditorGUILayout.Toggle("显示格子", Target.ShowGrids);

			this.curProcessType = (ProcessType)EditorGUILayout.EnumPopup("当前操作类型", this.curProcessType);
			if (this.curProcessType == ProcessType.Set || this.curProcessType == ProcessType.Clear)
			{
				this.curTileType = (MapNav.TileType)EditorGUILayout.EnumPopup("当前格子类型", this.curTileType);
				this.radius = EditorGUILayout.IntSlider("操作直径", this.radius, 1, 16);
			}
			else if (this.curProcessType == ProcessType.FindPath)
			{
				vecStart = EditorGUILayout.Vector3Field("起始点", vecStart);
				vecEnd = EditorGUILayout.Vector3Field("结束点", vecEnd);
			}
		}
		EditorGUILayout.EndToggleGroup();

		if (GUI.changed || Target.ShowGrids)
			EditorUtility.SetDirty(this.target);

		// 导出地图信息
		GUILayout.Space(20f);
		folds[2] = EditorGUILayout.BeginToggleGroup("导出地图信息", folds[2]);
		if (folds[2])
		{
			EditorGUILayout.Space();
			if (GUILayout.Button("Export"))
			{
				Export();
			}
		}
		EditorGUILayout.EndToggleGroup();

		serializedObject.ApplyModifiedProperties();
	}

	public void OnSceneGUI()
	{
		MapNav mapNav = Target;
		float y = 0.1f;
		if (curProcessType == ProcessType.None)
			return;

		if (curProcessType == ProcessType.Set || curProcessType == ProcessType.Clear)
		{
			Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
			float rayDistance;
			Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
			if (groundPlane.Raycast(ray, out rayDistance))
			{
				Vector3 hitPoint = ray.GetPoint(rayDistance);
				if (hitPoint.x >= 0.0f && hitPoint.x <= mapNav.gridXNum * mapNav.gridWidth
				   && hitPoint.z >= 0.0f && hitPoint.z <= mapNav.gridZNum * mapNav.gridHeight)
				{
					Handles.color = Color.black;
					float _radius = radius * 0.5f;
					Handles.DrawPolyLine(new Vector3[]
					{
						new Vector3(hitPoint.x - _radius * mapNav.gridWidth, y, hitPoint.z - _radius * mapNav.gridHeight),
						new Vector3(hitPoint.x - _radius * mapNav.gridWidth, y, hitPoint.z + _radius * mapNav.gridHeight),
						new Vector3(hitPoint.x + _radius * mapNav.gridWidth, y, hitPoint.z + _radius * mapNav.gridHeight),
						new Vector3(hitPoint.x + _radius * mapNav.gridWidth, y, hitPoint.z - _radius * mapNav.gridHeight),
						new Vector3(hitPoint.x - _radius * mapNav.gridWidth, y, hitPoint.z - _radius * mapNav.gridHeight)
					});

					if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
					{
						int x = mapNav.GetGridX(hitPoint);
						int z = mapNav.GetGridZ(hitPoint);
						for (int _z = z - Mathf.RoundToInt(_radius); _z <= z + Mathf.RoundToInt(_radius); ++_z)
						{
							if (_z < 0) continue;
							if (_z > mapNav.gridZNum - 1) continue;
							for (int _x = x - Mathf.RoundToInt(_radius); _x <= x + Mathf.RoundToInt(_radius); ++_x)
							{
								if (_x < 0) continue;
								if (_x > mapNav.gridXNum - 1) continue;

								Vector3 position = mapNav.GetWorldPosition(_x, _z);
								if (Mathf.Abs(position.x - hitPoint.x) <= _radius * mapNav.gridWidth
								   && Mathf.Abs(position.z - hitPoint.z) <= _radius * mapNav.gridHeight)
								{
									switch (curProcessType)
									{
										case ProcessType.Set:
											mapNav[_x, _z] |= curTileType;
											break;
										case ProcessType.Clear:
											mapNav[_x, _z] &= ~curTileType;
											break;
										default:
											throw new System.NotImplementedException();
									}
								}
							}
						}
					}
				}
			}
		}
		else if (curProcessType == ProcessType.FindPath)
		{
			if (Event.current.type == EventType.MouseDown)
			{
				Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
				float rayDistance;
				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				if (groundPlane.Raycast(ray, out rayDistance))
				{
					Vector3 hitPoint = ray.GetPoint(rayDistance);
					if (hitPoint.x >= 0.0f && hitPoint.x <= mapNav.gridXNum * mapNav.gridWidth
						&& hitPoint.z >= 0.0f && hitPoint.z <= mapNav.gridZNum * mapNav.gridHeight)
					{
						if (bSampleStart)
							vecStart = hitPoint;
						else
							vecEnd = hitPoint;
						bSampleStart = !bSampleStart;

						Cmd.Pos gridEnd = mapNav.FindNearestValidGrid(mapNav.GetGrid(vecStart), mapNav.GetGrid(vecEnd), MapNav.TileType.Walk);
						if(gridEnd != null)
							path = mapNav.GetPath(mapNav.GetGrid(vecStart), gridEnd, MapNav.TileType.Walk);
						else
							path.Clear();
					}
				}
			}

			Handles.color = Color.red;
			Handles.CubeCap(0, vecStart, new Quaternion(0, 0, 0, 1), 0.1f);

			Handles.color = Color.blue;
			Handles.CubeCap(0, vecEnd, new Quaternion(0, 0, 0, 1), 0.1f);

			if (path.Count > 0)
			{
				Handles.color = Color.black;
				Vector3 lastPoint = vecStart;
				lastPoint.y = y;
				foreach (Cmd.Pos grid in path)
				{
					Vector3 curPoint = mapNav.GetWorldPosition(grid);
					curPoint.y = y;
					Handles.color = Color.black;
					Handles.DrawLine(lastPoint, curPoint);
					lastPoint = curPoint;

					Handles.color = Color.cyan;
					Handles.CubeCap(0, curPoint, new Quaternion(0, 0, 0, 1), 0.1f);
				}
			}
		}

		HandleUtility.Repaint();
		switch (Event.current.type)
		{
			case EventType.MouseUp:
			case EventType.MouseDown:
			case EventType.MouseDrag:
			case EventType.MouseMove:
				Event.current.Use();
				break;
		}
	}

	void Export()
	{
		GX.Editor.LogEntries.Clear();
		var path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Common/data/map/" + Target.transform.parent.name);
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		if (ExportTile(path + "_tile.json") == false)
			return;
		if (ExportNpc(path + "_npc.json") == false)
			return;
		EditorUtility.DisplayDialog("MapNav Export OK", path + "_*.json", "OK");
	}

	/// <summary>
	/// 导出阻挡信息
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	private bool ExportTile(string path)
	{
		var json = NGUIJson.jsonEncode(new Hashtable()
		{
			//{"tilewidth", Target.gridWidth},
			//{"tileheight", Target.gridHeight},
			{"tilexnum", Target.gridXNum},
			{"tileynum", Target.gridZNum},
			{"tiles", System.Array.ConvertAll(Target.grids, g => (uint)g)},
		});
		//Debug.Log(json);
		File.WriteAllText(path, json, new System.Text.UTF8Encoding(false));
		return true;
	}

	/// <summary>
	/// 导出NPC摆放信息
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	private bool ExportNpc(string path)
	{
		var host = Target.transform.parent;
		var npc = host.GetComponentsInChildren<NpcEditor>();
		var db = Table.Query<table.TableNpc>().ToDictionary(i => i.id);

		var error = from i in npc let id = (uint)i.baseId where db.ContainsKey(id) == false select i;
		if (error.Any())
		{
			Debug.LogError("NPC表格中找不到对应id的NPC：\n" + string.Join("\n", (from i in error select string.Format("{0}\t{1}", i.baseId, i.transform.GetPath())).ToArray()));
			return false;
		}

		var json = NGUIJson.jsonEncode(new Hashtable()
		{
			{"npcs", System.Array.ConvertAll(npc, i => new Hashtable()
				{
					{"id", i.baseId},
					{"name", string.IsNullOrEmpty(i.alias) ? db[(uint)i.baseId].name : i.alias},
					{"x", Target.GetGridX(i.transform.localPosition)},
					{"y", Target.GetGridZ(i.transform.localPosition)},
					{"angle", (int)i.transform.localRotation.eulerAngles.y},
					{"relivetime", i.relivetime},
					{"rate", Mathf.Clamp(i.rate, 0, 100)},
				})
			},
		});
		//Debug.Log(json);
		File.WriteAllText(path, json, new System.Text.UTF8Encoding(false));
		return true;
	}
}
