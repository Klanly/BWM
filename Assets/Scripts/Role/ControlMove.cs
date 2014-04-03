using UnityEngine;
using System.Collections;

/// <summary>
/// 控制移动：键盘控制、触摸摇杆控制
/// </summary>
[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Move))]
public class ControlMove : MonoBehaviour {

	private Entity entity;
	private Move move;
	private MapNav MapNav { get { return BattleScene.Instance.MapNav; } }

	#region keyboard input
	private bool bKeyboardActive = false;
	private bool bUpState = false;
	private bool bDownState = false;
	private bool bLeftState = false;
	private bool bRightState = false;
	#endregion

	/// <summary>
	/// 客户端最大步伐
	/// </summary>
	public const float MaxClientMoveStep = 8.0f;

	// Use this for initialization
	void Start () {
		entity = this.gameObject.GetComponent<Entity>();
		move = this.gameObject.GetComponent<Move>();
	}

	/// <summary>
	/// 检测键盘输入控制移动
	/// </summary>
	public void MoveByKeyboard()
	{
		bUpState = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
		bDownState = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
		bLeftState = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
		bRightState = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

		if(!bUpState && !bDownState && !bLeftState && !bRightState)
		{
			if(bKeyboardActive)
			{
				bKeyboardActive = false;
				StopControl();
			}
			return;
		}

		bKeyboardActive = true;
		Vector3 vecDir = Camera.main.transform.rotation * Vector3.forward;
		vecDir.y = 0.0f;
		vecDir.Normalize();

		float fAngle = 0.0f;
		if (bUpState)
		{
			if(bLeftState)
				fAngle = -180 / 4.0f;
			else if(bRightState)
				fAngle = 180 / 4.0f;
		}
		else if(bDownState)
		{
			if(bLeftState)
				fAngle = -180 * 3.0f / 4.0f;
			else if(bRightState)
				fAngle = 180 * 3.0f / 4.0f;
			else
				fAngle = 180;
		}
		else
		{
			if(bLeftState)
				fAngle = -180 / 2.0f;
			else if(bRightState)
				fAngle = 180 / 2.0f;
		}

		Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(new Vector3(0, fAngle, 0)), Vector3.one);
		MoveWithDirection(mat.MultiplyVector(vecDir));
	}

	/// <summary>
	/// 朝指定方向向前移动
	/// </summary>
	/// <param name="vecDir">look rotation(= target - position)</param>
	void MoveWithDirection(Vector3 vecDir)
	{
		if(vecDir == Vector3.zero)
			return;

		vecDir.Normalize();

		// 如果当前方向和新方向一致，而且当前目标点还有一定距离达到，则继续原来的移动
		if(move.InMoving())
		{
			Vector3 vecCurDir = move.TargetPosition - entity.Position;
			vecCurDir.y = 0.0f;
			float fLength = vecCurDir.magnitude;
			vecCurDir.Normalize();
			if(Vector3.Dot(vecCurDir, vecDir) > 0.99985f && fLength > 0.5f )
			{
				return;
			}
		}

		// 寻找新目标点
		Vector3 vecNewPosition = entity.Position + vecDir * MaxClientMoveStep;
		Vector3 vecRealPosition;
		if(MapNav.IsPathReached(entity.Position, vecNewPosition, out vecRealPosition, entity.TileType))
		{
			move.TargetPosition = vecRealPosition;
			Debug.Log("发送移动消息:dst:" + move.TargetPosition + ",dir:" + this.transform.rotation);
			return;
		}
		else
		{
			// 稍微偏离方向再尝试
			int nNum = 10;
			float fDeltaRadian = 180 / 2.0f / nNum;
			float fRadian;
			for(int i = 1; i <= nNum; ++i)
			{
				// 先尝试左边
				fRadian = i * fDeltaRadian;
				Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(new Vector3(0, -1 * fRadian, 0)), Vector3.one);
				Vector3 vecNewDirection2 = mat.MultiplyVector(vecDir);
				Vector3 vecNewPosition2 = entity.Position + vecNewDirection2 * MaxClientMoveStep;
				Vector3 vecRealPosition2;
				if(MapNav.IsPathReached(entity.Position, vecNewPosition2, out vecRealPosition2, entity.TileType))
				{
					move.TargetPosition = vecRealPosition2;
					Debug.Log("发送移动消息:dst:" + move.TargetPosition + ",dir:" + this.transform.rotation);
					return;
				}

				// 在尝试右边
				mat.SetTRS(Vector3.zero, Quaternion.Euler(new Vector3(0, fRadian, 0)), Vector3.one);
				vecNewDirection2 = mat.MultiplyVector(vecDir);
				vecNewPosition2 = entity.Position + vecNewDirection2 * MaxClientMoveStep;
				if(MapNav.IsPathReached(entity.Position, vecNewPosition2, out vecRealPosition2, entity.TileType))
				{
					move.TargetPosition = vecRealPosition2;
					Debug.Log("发送移动消息:dst:" + move.TargetPosition + ",dir:" + this.transform.rotation);
					return;
				}
			}
		}
		return;
	}

	/// <summary>
	/// 停止控制：停止移动，发送最后位置给服务器
	/// </summary>
	void StopControl()
	{
		move.Stop();
		Debug.Log("发送停止移动消息:dst:" + entity.Position + ",dir:" + this.transform.rotation);
	}


}
