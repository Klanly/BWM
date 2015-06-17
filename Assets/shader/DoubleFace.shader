Shader "Custom/DoubleFace" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "" {}
	}

	SubShader{
		Pass{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			Alphatest Greater 0
			SetTexture[_MainTex] { combine texture }
		}
	}
}
