using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour
{
	public GXJoystick joystick;
	public CameraFollow cameraFollow;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (joystick)
		{
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			if (horizontal == 0 && vertical == 0)
			{
				horizontal = joystick.position.x;
				vertical = joystick.position.y;
			}

			MoveByJoystick(horizontal, vertical, joystick.pressed);
		}
	}

	/// <summary>
	/// 检测摇杆输入控制移动
	/// </summary>
	public void MoveByJoystick(float horizontal, float vertical, bool pressed)
	{
		Vector3 rootDirection = this.transform.forward;
		Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

		// Get camera rotation.    
		Vector3 CameraDirection = Camera.main.transform.forward;
		CameraDirection.y = 0.0f; // kill Y
		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

		// Convert joystick input in Worldspace coordinates
		Vector3 moveDirection = referentialShift * stickDirection;
		MoveWithDirection(moveDirection);
	}

	/// <summary>
	/// 朝指定方向向前移动
	/// </summary>
	/// <param name="vecDir">look rotation(= target - position)</param>
	void MoveWithDirection(Vector3 vecDir)
	{
		if (vecDir == Vector3.zero)
			return;
		vecDir.Normalize();

		this.transform.localRotation = Quaternion.Euler(0, -Mathf.Atan2(vecDir.z, vecDir.x) * Mathf.Rad2Deg + 90, 0);
		this.transform.localPosition += vecDir * Time.deltaTime * 3.0f;
		cameraFollow.UpdateCamera();
	}
}
