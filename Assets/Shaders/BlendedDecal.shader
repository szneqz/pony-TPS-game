Shader "Custom/BlendedDecal"
{
	Properties
	{
		_Color("Tint", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader
	{
		Lighting On
		ZTest LEqual
		ZWrite Off
		Tags{ "Queue" = "Transparent" }
		Pass
	{
		Alphatest Greater 0
		Blend SrcAlpha OneMinusSrcAlpha
		Offset -4, -4
		SetTexture[_MainTex]
	{
		ConstantColor[_Color]
		Combine texture * constant
	}
	}
	}
}