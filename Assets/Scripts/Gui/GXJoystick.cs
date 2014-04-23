using UnityEngine;
using System.Collections;

public class GXJoystick : MonoBehaviour {
	
	public float radius;
	public Vector3 scale = Vector3.one;
	private Plane mPlane;
	private Vector3 mLastPos;
	private Vector3 center;
	[HideInInspector]
	public Vector2 position;
	[HideInInspector]
	public bool pressed = false;

	private void Start ()
	{
		center = transform.localPosition;
	}
	
	/// <summary>
	/// Create a plane on which we will be performing the dragging.
	/// </summary>
	
	private void OnPress (bool pressed)
	{
		if (enabled && gameObject.activeInHierarchy) 
		{
			if (pressed)
			{
				mLastPos = UICamera.lastHit.point;
				mPlane = new Plane (Vector3.back, mLastPos);
			} 
			else 
			{
				position = Vector2.zero;
				if(this.transform.parent)
				{
					iTween.MoveTo(this.gameObject,transform.parent.position,1.0f);
				}
				else
				{
					transform.localPosition = center;
				}
			}
			this.pressed = pressed;
		}
	}
	
	/// <summary>
	/// Drag the object along the plane.
	/// </summary>
	
	void OnDrag (Vector2 delta)
	{
		if (enabled && gameObject.activeInHierarchy) 
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			
			Ray ray = UICamera.currentCamera.ScreenPointToRay (UICamera.currentTouch.pos);
			float dist = 0f;
			
			if (mPlane.Raycast (ray, out dist)) 
			{
				Vector3 currentPos = ray.GetPoint (dist);
				Vector3 offset = currentPos - mLastPos;
				mLastPos = currentPos;
				
				if (offset.x != 0f || offset.y != 0f) 
				{
					offset = transform.InverseTransformDirection (offset);
					offset.Scale (scale);
					offset = transform.TransformDirection (offset);
				}
				
				offset.z = 0;
				transform.position += offset;
				
				float length = transform.localPosition.magnitude;
				
				if (length > radius)
				{
					transform.localPosition = Vector3.ClampMagnitude (transform.localPosition, radius);
				}
				float x = (transform.localPosition.x-center.x)/radius;
				float y = (transform.localPosition.y-center.y)/radius;
				position = new Vector2(x,y);
			}
		}
	}
}
