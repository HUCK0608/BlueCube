Shader "BlueCube/WorldObject/TwoChange" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("3D_Texture", 2D) = "white" {}
		_MainTex4 ("2D_Texture2", 2D) = "white" {}
		_Choice("Choice", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex4;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex4;
		};

		float4 _Color;
		float _Emission;
		float _Speed;
		float _Choice;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			float4 e = tex2D(_MainTex4, IN.uv_MainTex4);
						
			if (_Choice == 0)
			{
				o.Albedo = c.rgb;
			}

			else if (_Choice == 1)
			{
				o.Albedo = e.rgb;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
