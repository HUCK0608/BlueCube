Shader "BlueCube/WorldObject/ShiningTile" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Main_Texture", 2D) = "white" {}
		_MainTex3 ("Spectrum", 2D) = "white" {}
		_Emission("Emission_Power", float) = 1
		_Speed("Emission_Speed", float) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
		};

		float4 _Color;
		float _Emission;
		float _Speed;
		float _Check;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			float4 d = tex2D(_MainTex3, float2(_Time.y * _Speed, 0.5));
						
			o.Albedo = c.rgb;
			o.Emission = (o.Emission + _Color) * _Emission * d.r;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
