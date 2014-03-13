using UnityEngine;
using System.Collections;

public class MapTexture : MonoBehaviour
{
	public Texture texture;
	public int pixelsInXOneMeter = 64;
	public int pixelsInZOneMeter = 32;

	// Use this for initialization
	void Start()
	{
		Reset();
	}

	public void Reset()
	{
		if (texture == null)
			return;
		int width = texture.width / pixelsInXOneMeter;
		int height = texture.height / pixelsInZOneMeter;

		transform.localScale = new Vector3(width, height, 1);
		transform.localEulerAngles = new Vector3(90, 0, 0);
		transform.position = new Vector3(width / 2.0f, 0, height / 2.0f);

		renderer.sharedMaterial = new Material(Shader.Find("Unlit/Texture")) { mainTexture = texture, name = texture.name };
	}
}
