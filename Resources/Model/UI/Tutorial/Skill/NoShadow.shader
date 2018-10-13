Shader "Custom/NoShadow" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		CGPROGRAM
		#pragma surface surf NoLight noshadow

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color.rgb;
			o.Alpha = 1;
		}

		float4 LightingNoLight(SurfaceOutput o, float3 lightDir, float atten)
		{
			float4 finalColor;
			finalColor.rgb = o.Albedo;
			finalColor.a = 1;

			return finalColor;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
