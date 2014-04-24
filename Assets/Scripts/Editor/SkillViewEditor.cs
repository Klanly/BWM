using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(SkillView))]
public class SkillViewEditor : Editor {

	SkillView skillView;
	private SerializedProperty startGo;
	private SerializedProperty targetGo;
	private SerializedProperty skillGo;
	private SerializedProperty buffGo;
	
	void OnEnable()
	{
		skillView = target as SkillView;
		startGo = serializedObject.FindProperty("startGo");
		targetGo = serializedObject.FindProperty("targetGo");
		skillGo = serializedObject.FindProperty("skillGo");
		buffGo = serializedObject.FindProperty("buffGo");
	}
	
	public override void OnInspectorGUI ()
	{
		EditorGUILayout.PropertyField(startGo, new GUIContent("施法者"));
		EditorGUILayout.PropertyField(targetGo, new GUIContent("被击者"));
		EditorGUILayout.PropertyField(skillGo, new GUIContent("Skill"));
		EditorGUILayout.PropertyField(buffGo, new GUIContent("Buff"));

		if(GUILayout.Button("Start Skill"))
			skillView.startSkill();
		if(GUILayout.Button("Start Buff"))
			skillView.startBuff();

		serializedObject.ApplyModifiedProperties();
	}
}
