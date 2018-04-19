Shader "BlueCube/WorldObject/CanChange" {
	Properties {
		_MainTex ("default_UV", 2D) = "white" {}
		_MainTex2 ("Emission_UV", 2D) = "white" {}
		_MainTex3 ("Spectrum", 2D) = "white" {}
		_Emission ("Emission_Power", float) = 1
		_Speed("Emission_Speed", float) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Opaque"="Transparent" }
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
			float3 worldPos;
			float3 worldNormal;
		};

		float _Emission;
		float _Speed;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			float4 d = tex2D(_MainTex3, float2(_Time.y * _Speed, 0.5));

			float2 topUV = float2(IN.worldPos.x, IN.worldPos.z);
			float2 frontUV = float2(IN.worldPos.x, IN.worldPos.y);
			float2 sideUV = float2(IN.worldPos.z, IN.worldPos.y);

			float4 topTex = tex2D(_MainTex2, topUV);
			float4 frontTex = tex2D(_MainTex2, frontUV);
			float4 sideTex = tex2D(_MainTex2, sideUV);

			o.Albedo = c.rgb;
			o.Emission = lerp(topTex, frontTex, abs(IN.worldNormal.z));
			o.Emission = lerp(o.Emission, sideTex, abs(IN.worldNormal.x));
			o.Emission = o.Emission * _Emission * d.r;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
