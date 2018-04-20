Shader "BlueCube/WorldObject/SpecularShining" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("MainTexture", 2D) = "white" {}
		_MainTex2("SpeculerTexture", 2D) = "white" {}
		_SpeculerSize("SpeculerSize", float) = 1
		_SpeculerVector("Speculer_Dir", Vector) = (0,0,0,0)
		_SpeculerRange("Speculer_range", Range(0,10)) = 7
		_EmissionPower("Emission_Power", float) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		float4 _Color;


		void surf (Input IN, inout SurfaceOutputStandard o) {
			float4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Albedo = c.rgb;
		}
		ENDCG

		CGPROGRAM
		#pragma surface surf WS alpha:fade
		#pragma target 3.0

		sampler2D _MainTex2;

		struct Input {
			float3 worldPos;
		};

		float4 _Color;
		float _SpeculerSize;
		float4 _SpeculerVector;
		float _SpeculerRange;
		float _EmissionPower;
		float4 d;


		void surf(Input IN, inout SurfaceOutput o) {
			float2 top_UV = float2(IN.worldPos.x, IN.worldPos.z);

			d = tex2D(_MainTex2, top_UV * _SpeculerSize);

			o.Emission = d.rgb + _EmissionPower;
			o.Alpha = d.r;
		}

		float4 LightingWS(SurfaceOutput s, float3 lightDir, float3 viewDir, float3 atten)
		{
			float3 vnorl = saturate(dot(s.Normal, normalize(_SpeculerVector.rgb + viewDir)));

			vnorl = pow(vnorl, pow(10, _SpeculerRange));

			float4 final_return;
			final_return.rgb = s.Emission * vnorl * _LightColor0.rgb;
			final_return.a = s.Alpha;
			if (final_return.a != 0)
				final_return.a = lerp(0, 1, vnorl.r);
			else
				final_return.a = 0;

			return final_return;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
