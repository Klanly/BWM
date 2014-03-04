using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapNav))]
public class MapNavEditor : Editor
{
	private SerializedProperty gridWidth;
	private SerializedProperty gridHeight;
	private SerializedProperty gridXNum;
	private SerializedProperty gridZNum;
	private SerializedProperty showGrids;
	private SerializedProperty curTileType;
	private SerializedProperty curProcessType;
	private SerializedProperty radius;

	public MapNav Target { get { return (MapNav)target; } }

	void OnEnable()
	{
		gridWidth = serializedObject.FindProperty("gridWidth");
		gridHeight = serializedObject.FindProperty("gridHeight");
		gridXNum = serializedObject.FindProperty("gridXNum");
		gridZNum = serializedObject.FindProperty("gridZNum");
		showGrids = serializedObject.FindProperty("showGrids");
		curTileType = serializedObject.FindProperty("curTileType");
		curProcessType = serializedObject.FindProperty("curProcessType");
		radius = serializedObject.FindProperty("radius");
	}

	public override void OnInspectorGUI()
	{
		Tools.current = Tool.View;

		EditorGUILayout.Space();
		serializedObject.Update();

		EditorGUILayout.PropertyField(gridWidth, new GUIContent("格子宽(米)"));
		EditorGUILayout.PropertyField(gridHeight, new GUIContent("格子高(米)"));
		EditorGUILayout.PropertyField(gridXNum, new GUIContent("X轴格子数"));
		EditorGUILayout.PropertyField(gridZNum, new GUIContent("Z轴格子数"));
		if (GUILayout.Button("Update"))
		{
			Target.Reset();
		}

		EditorGUILayout.PropertyField(showGrids, new GUIContent("显示格子"));
		EditorGUILayout.PropertyField(curProcessType, new GUIContent("当前操作类型"));
		EditorGUILayout.PropertyField(curTileType, new GUIContent("当前格子类型"));
		EditorGUILayout.IntSlider(radius, 1, 16, "操作直径");

		serializedObject.ApplyModifiedProperties();
	}

	public void OnSceneGUI()
	{
		MapNav mapNav = Target;
		float y = 0.1f;
		if (mapNav.curProcessType == MapNav.ProcessType.None)
			return;

		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		float rayDistance;
		Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		if (groundPlane.Raycast(ray, out rayDistance))
		{
			Vector3 hitPoint = ray.GetPoint(rayDistance);
			if (hitPoint.x >= 0.0f && hitPoint.x <= mapNav.gridXNum * mapNav.gridWidth
			   && hitPoint.z >= 0.0f && hitPoint.z <= mapNav.gridZNum * mapNav.gridHeight)
			{
				Handles.color = Color.white;
				float _radius = mapNav.radius * 0.5f;
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
					int x = mapNav.getX(hitPoint);
					int z = mapNav.getZ(hitPoint);
					for (int _z = z - Mathf.RoundToInt(_radius); _z <= z + Mathf.RoundToInt(_radius); ++_z)
					{
						if (_z < 0) continue;
						if (_z > mapNav.gridZNum - 1) continue;
						for (int _x = x - Mathf.RoundToInt(_radius); _x <= x + Mathf.RoundToInt(_radius); ++_x)
						{
							if (_x < 0) continue;
							if (_x > mapNav.gridXNum - 1) continue;

							Vector3 position = mapNav.getPosition(_x, _z);
							if (Mathf.Abs(position.x - hitPoint.x) <= _radius * mapNav.gridWidth
							   && Mathf.Abs(position.z - hitPoint.z) <= _radius * mapNav.gridHeight)
							{
								switch ((MapNav.ProcessType)curProcessType.intValue)
								{
									case MapNav.ProcessType.None:
										break;
									case MapNav.ProcessType.Set:
										mapNav[_x, _z] |= mapNav.curTileType;
										break;
									case MapNav.ProcessType.Clear:
										mapNav[_x, _z] &= ~mapNav.curTileType;
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
}
