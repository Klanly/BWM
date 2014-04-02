// 遮罩渲染
// http://blog.csdn.net/lzhq1982/article/details/12783493
Shader "Custom/Mask" {
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Mask("Culling Mask", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0, 1)) = 0.1
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		Lighting Off
		ZWrite Off
		Blend Off
		AlphaTest GEqual[_Cutoff]
		Pass
		{
			SetTexture[_Mask] {combine texture}
			SetTexture[_MainTex] {combine texture, previous}
		}
	}
}
