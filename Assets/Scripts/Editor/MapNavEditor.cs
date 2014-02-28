using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapNav))]
public class MapNavEditor : Editor {

	private SerializedProperty gridWidth;
	private SerializedProperty gridHeight;
	private SerializedProperty gridXNum;
	private SerializedProperty gridZNum;
	private SerializedProperty showGrids;

	void OnEnable()
	{
		gridWidth = serializedObject.FindProperty("gridWidth");
		gridHeight = serializedObject.FindProperty("gridHeight");
		gridXNum = serializedObject.FindProperty("gridXNum");
		gridZNum = serializedObject.FindProperty("gridZNum");
		showGrids = serializedObject.FindProperty("showGrids");
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.Space();
		serializedObject.Update();

		EditorGUILayout.PropertyField(gridWidth, new GUIContent("gridWidth"));
		EditorGUILayout.PropertyField(gridHeight, new GUIContent("gridHeight"));
		EditorGUILayout.PropertyField(gridXNum, new GUIContent("gridXNum"));
		EditorGUILayout.PropertyField(gridZNum, new GUIContent("gridZNum"));
		EditorGUILayout.PropertyField(showGrids, new GUIContent("showGrids"));
		serializedObject.ApplyModifiedProperties();

		if (GUILayout.Button("Update"))
		{
			((MapNav)target).Reset();
		}
	}

	public void OnSceneGUI () {
		if(showGrids.boolValue)
		{
			int _gridZNum = gridZNum.intValue;
			int _gridXNum = gridXNum.intValue;
			float _gridWidth = gridWidth.floatValue;
			float _gridHeight = gridHeight.floatValue;

			for(int z = 0; z < _gridZNum; ++z)
			{
				for(int x = 0; x < _gridXNum; ++x)
				{
					Vector3[] verts = {new Vector3(x*_gridWidth, 0.5f, z*_gridHeight),
						new Vector3(x*_gridWidth, 0.5f, (z+1)*_gridHeight),
						new Vector3((x+1)*_gridWidth, 0.5f, (z+1)*_gridHeight),
						new Vector3((x+1)*_gridWidth, 0.5f, z*_gridHeight)};

					Handles.DrawSolidRectangleWithOutline(verts, new Color(1,1,1,0.2f), new Color(0,0,0,1));
				}
			}
		}
	}
}
