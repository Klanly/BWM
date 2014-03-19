using UnityEngine;
using System.Collections;

public class CameraFollow: MonoBehaviour {

	public float distanceCameraToRole = 25.0f;
	public float heightCameraLookAt = 0.5f;

	private Camera cameraMain;
	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }


	// Use this for initialization
	void Start () {
		cameraMain = GameObject.Find("CameraMain").GetComponent<Camera>();
	}
	
	/// <summary>
	/// Updates the camera.
	/// </summary>
	public void UpdateCamera()
	{
		if (cameraMain == null || MapNav == null)
			return;
		var targetCenter = this.transform.position;
		targetCenter.z += heightCameraLookAt;
		
		var dx = cameraMain.orthographicSize * Screen.width / Screen.height;
		var dz = cameraMain.orthographicSize / Mathf.Sin(cameraMain.transform.rotation.eulerAngles.x * Mathf.Deg2Rad) + heightCameraLookAt;
		
		targetCenter.x = Mathf.Clamp(targetCenter.x, dx, MapNav.transform.localScale.x - dx);
		targetCenter.z = Mathf.Clamp(targetCenter.z, dz, MapNav.transform.localScale.y - dz);
		
		var pos = targetCenter + cameraMain.transform.rotation * Vector3.back * distanceCameraToRole;
		cameraMain.transform.position = pos;
	}
}
