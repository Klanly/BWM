using UnityEngine;
using System.Collections;

public class Terrain2D : MonoBehaviour
{
	public Texture texture;
	public int pixelsInXOneMeter = 64;
	public int pixelsInZOneMeter = 32;

	// Use this for initialization
	void Start()
	{
		if (texture != null)
		{
			int width = texture.width / pixelsInXOneMeter;
			int height = texture.height / pixelsInZOneMeter;

			transform.localScale = new Vector3(width, height, 1);
			transform.localEulerAngles = new Vector3(90, 0, 0);
			transform.position = new Vector3(width / 2.0f, 0, height / 2.0f);

			var tempMaterial = new Material(renderer.sharedMaterial);
			tempMaterial.mainTexture = texture;
			tempMaterial.shader = Shader.Find("Unlit/Texture");
			renderer.sharedMaterial = tempMaterial;
		}
	}

	public void Reset()
	{
		Start();
	}
}
