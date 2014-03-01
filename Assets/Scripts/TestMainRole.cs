using UnityEngine;
using System.Collections;

public class TestMainRole : MonoBehaviour {
	public float speedMainRole = 5.0f;
	public float distanceCameraToRole = 25.0f;
	public float heightCameraLookAt = 0.5f;

	private Transform mainRole;
	private Transform birthPos;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(mainRole == null || birthPos == null)
		{
			if(GameObject.Find("BirthPos"))
				birthPos = GameObject.Find("BirthPos").transform;
			if(GameObject.Find("MainRole").transform)
				mainRole = GameObject.Find("MainRole").transform;
			if(mainRole && birthPos)
				mainRole.position = birthPos.position;
		}


		// 设置主角的移动
		if(mainRole)
		{
			// keyboard
			float v = Input.GetAxisRaw("Vertical");
			float h = Input.GetAxisRaw("Horizontal");

			Vector3 oldPosition = mainRole.transform.position;
			oldPosition.x += h * speedMainRole * Time.deltaTime;
			oldPosition.z += v * speedMainRole * Time.deltaTime;
			mainRole.position = oldPosition;

			// 触摸屏
			if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)) {
				Collider terrain = GameObject.Find("Terrain2D").collider;
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit hit;
				if (terrain.Raycast(ray, out hit, 1000))
				{
					Vector3 direction = hit.point - mainRole.position;
					direction.y = 0;
					mainRole.position = mainRole.position + Vector3.Normalize(direction) * speedMainRole * Time.deltaTime;
				}
			}

			// mouse
			if (Input.GetMouseButton(0)) {				
				Collider terrain = GameObject.Find("Terrain2D").collider;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (terrain.Raycast(ray, out hit, 1000))
				{
					Vector3 direction = hit.point - mainRole.position;
					direction.y = 0;
					mainRole.position = mainRole.position + Vector3.Normalize(direction) * speedMainRole * Time.deltaTime;
				}
			}
		}			

		// 设置照相机位置
		if(mainRole)
		{
			Vector3 targetCenter = mainRole.position;
			targetCenter.y += heightCameraLookAt;
			this.transform.position = targetCenter;
			float currentAngle = this.transform.eulerAngles.y;
			this.transform.position += this.transform.rotation * Vector3.back * distanceCameraToRole;
		}
	}
}
