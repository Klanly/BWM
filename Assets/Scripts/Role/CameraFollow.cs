using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
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
		var pos = this.transform.position;
		pos.y += 2.1f;
		pos.z += -2.1f;
		Camera.main.transform.position = pos;
	}
}
