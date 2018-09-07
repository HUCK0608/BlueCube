Shader "Custom/PowerLine_Line" {
	Properties {
		[HDR]_OnColor ("OnColor", Color) = (1, 1, 1, 1)
		_OffColor("OffColor", Color) = (0, 0, 0, 0)
		[Toggle]_Brightness("Brightness", float) = 0
		[HideInInspector]_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		float4 _OnColor;
		float4 _OffColor;
		float _Brightness;
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = float3(0, 0, 0);
			o.Alpha = 1;

			float uvXStepFill = step(IN.uv_MainTex.y, _Brightness);
			o.Emission = uvXStepFill * _OnColor.rgb + (1 - uvXStepFill) * _OffColor.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
