using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
	public Vector3 distance = new Vector3(0, 2, -2);
	// Use this for initialization
	void Start()
	{
		UpdateCamera();
	}

	/// <summary>
	/// 以宿主为中心，更新摄像机位置
	/// </summary>
	public void UpdateCamera()
	{
		Camera.main.transform.position = this.transform.position + distance;
	}
}
