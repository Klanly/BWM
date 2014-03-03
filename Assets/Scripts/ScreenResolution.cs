using UnityEngine;
using System.Collections;

public class ScreenResolution : MonoBehaviour
{
	public int width = 960;
	public int height = 640;

	void Start()
	{
		var pc = Application.platform == RuntimePlatform.WindowsPlayer
			|| Application.platform == RuntimePlatform.LinuxPlayer
			|| Application.platform == RuntimePlatform.OSXPlayer;

		int _height = height;
		int _width = height * Screen.width / Screen.height;	//保证手机的aspect不变
		//Debug.LogError(string.Format("width:{0},height:{1}",_width,_height));
		Screen.SetResolution(_width, _height, !pc);
	}
}
