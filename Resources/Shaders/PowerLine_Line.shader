Shader "Custom/PowerLine_Line" {
	Properties {
		_OnColor ("OnColor", Color) = (1, 1, 1, 1)
		_OffColor("OffColor", Color) = (0, 0, 0, 0)
		_Fill("Fill", Range(0, 1)) = 0
		[Toggle]_Reverse("Reverse", float) = 0
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		float4 _OnColor;
		float4 _OffColor;
		float _Fill;
		float _Reverse;
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = float3(0, 0, 0);
			o.Alpha = 1;

			float uvXStepFill = abs(step(IN.uv_MainTex.y, abs(_Fill - _Reverse)) - _Reverse);
			o.Emission = uvXStepFill * _OnColor.rgb + (1 - uvXStepFill) * _OffColor.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
