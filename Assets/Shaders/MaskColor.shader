// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/MaskColor" {
	Properties {
		[PerRendererData]_Color1 ("hair_BG", Color) = (1,1,1,1)
		[PerRendererData]_Color2 ("hair_one", Color) = (1,1,1,1)
		[PerRendererData]_Color3 ("hair_two", Color) = (1,1,1,1)
		[PerRendererData]_Color4 ("body", Color) = (1,1,1,1)
		[PerRendererData]_Color5 ("eyes", Color) = (1,1,1,1)
	//	_ColorClear ("Color", Color) = (0.1, 0.1, 0.1, 1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_GSTex ("GrayScale", 2D) = "white" {}
		[PerRendererData]_CMTex("CM", 2D) = "white" {}
		_MaETex("Mouth_Eyes", 2D) = "white" {}
		[PerRendererData]_MaskTex_1a("Tail", 2D) = "white" {}
		[PerRendererData]_MaskTex_1b("BackHair", 2D) = "white" {}
		[PerRendererData]_MaskTex_1c("FrontHair", 2D) = "white" {}
		[PerRendererData]_MaskTex_2("Body & Eyes", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex, _MaskTex_1a, _MaskTex_1b, _MaskTex_1c, _MaskTex_2, _GSTex, _CMTex, _MaETex;

		struct Input {
			float2 uv_MainTex;//, uv_MaskTex_1, uv_MaskTex_2, uv_GSTex;	//nie wiem po co to potem usune
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;
		fixed4 _Color4;
		fixed4 _Color5;
	//	fixed4 _ColorClear;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);			//podstawoa tekstura
			fixed4 mask_1a = tex2D(_MaskTex_1a, IN.uv_MainTex);	//TAIL
			fixed4 mask_1b = tex2D(_MaskTex_1b, IN.uv_MainTex);	//BACKHAIR
			fixed4 mask_1c = tex2D(_MaskTex_1c, IN.uv_MainTex);	//FRONTHAIR

			half4 mainTexVisible = mask_1a.rgba * ( mask_1a.a); //- (0,0,0,mask_1a.a);	//lacze TAIL i BACKHAIR
			half4 overlayTexVisible = mask_1b.rgba * (mask_1b.a);	//
																	//
			fixed4 finalColor = (mainTexVisible + overlayTexVisible);//

			mainTexVisible = finalColor.rgba * (1 - mask_1c.a);		//lacze polaczone ^ oraz FRONTHAIR
			overlayTexVisible = mask_1c.rgba * (mask_1c.a);			//

			fixed4 mask_1 = (mainTexVisible + overlayTexVisible);	//
			float cmask_1 = min(1.0f, mask_1.r + mask_1.g + mask_1.b + mask_1.a);	//jakies ograniczenie dzieki ktoremu to sie kupy trzyma

			fixed4 mask_2 = tex2D(_MaskTex_2, IN.uv_MainTex);		//cialo oraz oczy

			float cmask_2 = min(1.0f, mask_2.r + mask_2.g + mask_2.b + mask_2.a);	//jakies ograniczenie dzieki ktoremu to sie kupy trzyma

			//cmask_1 = cmask_1 - cmask_2;

			fixed4 grayScale = tex2D(_GSTex, IN.uv_MainTex);		//greyscale
			c.rgb = (c.rgb * (1 - cmask_1 + 1 - cmask_2) * fixed4(0, 0, 0, 0) + ((_Color1 * mask_1.r + _Color2 * mask_1.g + _Color3 * mask_1.b) * mask_1.a) + ((_Color4 *  mask_2.r + _Color5 *  mask_2.g) * mask_2.a)) * grayScale;
			//wyczepiasty skrypt, ktory napisalem i jakos dziala
			fixed4 cmTex = tex2D(_CMTex, IN.uv_MainTex);	//CMka

			mainTexVisible = c.rgba * (1 - cmTex.a);
			overlayTexVisible = cmTex.rgba * (cmTex.a);

			c.rgba = (mainTexVisible + overlayTexVisible);	//polaczona z caloscia

			fixed4 maeTex = tex2D(_MaETex, IN.uv_MainTex);	//usta i oczy

			mainTexVisible = c.rgba * (1 - maeTex.a);
			overlayTexVisible = maeTex.rgba * (maeTex.a);

			c.rgba = (mainTexVisible + overlayTexVisible);	//polaczona z caloscia

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
