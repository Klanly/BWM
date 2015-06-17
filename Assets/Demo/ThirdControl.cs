using UnityEngine;
using System.Collections;

/// <summary>
/// ref: http://www.cnblogs.com/He-Jing/p/3792690.html
/// </summary>
public class ThirdControl : MonoBehaviour
{
	public Transform target;//要跟随的目标
	public float distance = 8.0f;//摄像机离目标的距离
	public float height = 5.0f;//摄像机离目标的高度
	public float heightDamping = 0.3f;//水平跟随平滑系数
	public float rotationDamping = 0.3f;//跟随高度变化系数
	public float refRotation = 0f;
	public float refHeight = 0f;

	void LateUpdate()
	{
		if (target)
		{
			float targetRotationAngle = target.eulerAngles.y;//目标的朝向
			float targetHeight = target.position.y + height;//得到跟随的高度

			float cameraRotationAngle = transform.eulerAngles.y;//摄像机的朝向
			float cameraHeight = transform.position.y;//摄像机的高度

			cameraRotationAngle = Mathf.SmoothDampAngle(cameraRotationAngle, targetRotationAngle, ref refRotation, rotationDamping);//从摄像机目前的角度变换到目标的角度

			cameraHeight = Mathf.SmoothDamp(cameraHeight, targetHeight, ref refHeight, heightDamping);//从摄像机目前的高度平滑变换到目标的高度

			Quaternion cameraRotation = Quaternion.Euler(0, cameraRotationAngle, 0);//每帧在Y轴上旋转摄像机 旋转的角度为cameraRotationAngle 因为上面的代码已经得到了每帧要从摄像机当前的角度变换到目标角度cameraRotationAngle

			//下面几句代码主要设置摄像机的位置
			transform.position = target.position;
			transform.position -= cameraRotation * Vector3.forward * distance;
			transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z);

			//使摄像机一直朝着目标方向
			transform.LookAt(target);
		}
	}
}