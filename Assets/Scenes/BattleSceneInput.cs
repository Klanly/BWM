using UnityEngine;
using System.Collections;

/// <summary>
/// 战斗场景输入处理
/// </summary>
/// <remarks>
/// NGUI: Events
/// ref: http://www.tasharen.com/?page_id=160
/// </remarks>
public class BattleSceneInput : MonoBehaviour
{
	public Role Role { get { return MainRole.Instance.Role; } }
	public MapNav MapNav { get { return BattleScene.Instance.MapNav; } }
	private bool pressing = false;

	void Start()
	{
		UICamera.fallThrough = this.gameObject;
	}

	void Destroy()
	{
		UICamera.fallThrough = null;
	}

	/// <summary>
	/// Sent when a mouse button (or touch event) gets pressed over the collider (with ‘true’)
	/// and when it gets released (with ‘false’, sent to the same collider even if it’s released elsewhere).
	/// </summary>
	/// <param name="isDown"></param>
	void OnPress(bool isDown)
	{
		pressing = isDown;
	}

	void Update()
	{
		if (pressing)
		{
			var ray = Camera.main.ScreenPointToRay(new Vector3(UICamera.lastTouchPosition.x, UICamera.lastTouchPosition.y));
			Collider terrain = MapNav.gameObject.collider;
			RaycastHit hit;
			if (terrain.Raycast(ray, out hit, 1000))
			{
				Role.TargetPosition = hit.point;
			}
		}
	}

	/// <summary>
	/// is sent when keyboard or controller input is used.
	/// </summary>
	/// <param name="key"></param>
	void OnKey(KeyCode key)
	{
		var distance = 2;
		switch(key)
		{
			case KeyCode.UpArrow:
				Role.TargetPosition = Role.Position + new Vector3(0, 0, +distance);
				break;
			case KeyCode.DownArrow:
				Role.TargetPosition = Role.Position + new Vector3(0, 0, -distance);
				break;
			case KeyCode.LeftArrow:
				Role.TargetPosition = Role.Position + new Vector3(-distance, 0, 0);
				break;
			case KeyCode.RightArrow:
				Role.TargetPosition = Role.Position + new Vector3(+distance, 0, 0);
				break;
			default:
				break;
		}
	}
}
