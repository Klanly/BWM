using UnityEngine;
using System.Collections;
using Cmd;
using GX.Net;

/// <summary>
/// 战斗场景输入处理
/// </summary>
/// <remarks>
/// NGUI: Events
/// ref: http://www.tasharen.com/?page_id=160
/// </remarks>
public class BattleSceneInput : MonoBehaviour
{
	public MainRole mainRole { get { return MainRole.Instance; } }
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
		if (isDown)
		{
			var ray = Camera.main.ScreenPointToRay(new Vector3(UICamera.lastTouchPosition.x, UICamera.lastTouchPosition.y));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (SelectTarget.Select(hit.collider.gameObject) == false)
				{
					// 没有选中目标，就移动
					if (hit.collider.gameObject.GetComponent<MapNav>())
					{
						mainRole.pathMove.WalkTo(hit.point);
					}
				}
			}
		}

		pressing = isDown;
	}

	void Update()
	{
		// Esc关闭界面或退出场景
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			var topmost = Closeable.TopMost();
			if (topmost != null)
			{
				topmost.gameObject.SetActive(false);
			}
			else
			{
				Net.Instance.Send(new CharactorUnregSelectUserCmd_CS() { });
			}
		}
	}
	[Execute]
	public static void Execute(CharactorUnregSelectUserCmd_CS cmd)
	{
		Application.LoadLevel("RoleListScene");
	}
	/// <summary>
	/// is sent when keyboard or controller input is used.
	/// </summary>
	/// <param name="key"></param>
	void OnKey(KeyCode key)
	{
		// 键盘控制角色移动
		if (mainRole != null)
		{
			mainRole.controlMove.MoveByKeyboard();
		}
	}
}
