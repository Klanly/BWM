using UnityEngine;
using System.Collections;

public class PrefabLoader : MonoBehaviour
{
	public string[] prefabs;

	void Start()
	{
		foreach (var p in prefabs)
		{
			LoadPrefab(p);
		}
	}

	private GameObject LoadPrefab(string path)
	{
		var r = Resources.Load(path);
		if (r == null)
		{
			Debug.LogWarning("Can't load prefab: " + path);
			return null;
		}
		var go = GameObject.Instantiate(r) as GameObject;
		if (go == null)
		{
			Debug.LogError("Can't instant GameObject from prefab: " + path);
			return null;
		}
		go.transform.parent = this.transform;
		go.transform.localEulerAngles = Vector3.one;
		go.transform.position = Vector3.zero;
		return go;
	}
}
