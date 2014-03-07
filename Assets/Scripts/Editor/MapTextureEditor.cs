using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapTexture))]
public class MapTextureEditor : Editor {

	private SerializedProperty texture;
	private SerializedProperty pixelsInXOneMeter;
	private SerializedProperty pixelsInZOneMeter;

	void OnEnable()
	{
		texture = serializedObject.FindProperty("texture");
		pixelsInXOneMeter = serializedObject.FindProperty("pixelsInXOneMeter");
		pixelsInZOneMeter = serializedObject.FindProperty("pixelsInZOneMeter");
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.Space();
		serializedObject.Update();

		// texture
		EditorGUILayout.PropertyField(texture, new GUIContent("texture"));

		// pixelsInXOneMeter
		EditorGUILayout.PropertyField(pixelsInXOneMeter, new GUIContent("pixelsInXOneMeter"));
		
		// pixelsInZOneMeter
		EditorGUILayout.PropertyField(pixelsInZOneMeter, new GUIContent("pixelsInZOneMeter"));

		serializedObject.ApplyModifiedProperties();

		if (GUILayout.Button("Update"))
		{
			((MapTexture)target).Reset();
		}
	}
}
