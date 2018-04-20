Shader "BlueCube/ToonShader" {
	Properties {		
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ShadowCtrl("Shadow_Control", Range(0,1)) = 0.5
		/*_SpecularCtrl("Specular_Control", float) = 1
		_SpecularCtrl2("Specular_Control2", Range(0.8,1)) = 1
		_Color("Specular_Color", Color) = (1,1,1,1)*/
		_Ramp("RampColor", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0,1)) = 0.7
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		/*Stencil{
			Ref 2
			Comp always
			pass replace
		}*/

		CGPROGRAM
		#pragma surface surf Toon

		#pragma target 3.0

		sampler2D _MainTex;

		float _ShadowCtrl;
		/*float _SpecularCtrl;
		float _SpecularCtrl2;
		float4 _Color;*/
		float4 _Ramp;
		float _Brightness;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}


		/*float4 LightingToon(SurfaceOutput s, float3 lightDir, float3 viewDir, float3 atten)
		{
			float ndotl = dot(s.Normal,lightDir) * 0.5 + 0.5;

			if(ndotl < _ShadowCtrl)
				ndotl = 0;

			float ndotl2;

			float3 ndotl_final = ndotl * s.Albedo * _LightColor0.rgb;

			float lnorv = saturate(dot(normalize(lightDir + viewDir),s.Normal));

			if(lnorv < _SpecularCtrl2)
			lnorv = 0;
			else
			lnorv = 1;

			lnorv = pow(lnorv, _SpecularCtrl);

			float3 lnorv_final = lnorv * _Color.rgb;

			float4 final;
			final.rgb = ndotl_final.rgb + lnorv_final.rgb;			
			final.a = s.Alpha;
			return final;
		}*/

		float4 LightingToon(SurfaceOutput s, float3 lightDir, float3 viewDir, float3 atten)
		{
			float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;			
			float4 final_ndotl;
			
			if (ndotl < _ShadowCtrl)
				ndotl = 0;
			else
				ndotl = 1;

			if (ndotl < 1)
			{
				final_ndotl.rgb = _Ramp.rgb;
			}
			else
				final_ndotl.rgb = _Brightness * s.Albedo * _LightColor0.rgb;

			return final_ndotl;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
