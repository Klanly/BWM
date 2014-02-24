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
		Screen.SetResolution(960, 640, !pc);
	}
}
