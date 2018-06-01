Shader "BlueCube/FirePillar/NoFixFire_Water" {
	Properties {
		_MainTex ("back", 2D) = "white" {}
		_MainTex3 ("front", 2D) = "white" {}
		_MainTex2 ("일렁일렁", 2D) = "white" {}
		[HideinInspector]_Cutoff("Cutoff",float) = 0.5
		_WaterHeight("물 높이",Range(0, 1)) = 0.33
	}
	SubShader {
		Tags { "RenderType"="TransparentCutout" " Queue" = "AlphaTest"}
		LOD 200
		cull off
	
		//1pass
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alphatest:_Cutoff addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;

		float _WaterHeight;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float waterHeight = clamp(1 - _WaterHeight, 0.33, 0.95);
			float4 disto = tex2D (_MainTex2, float2(IN.uv_MainTex2.x+_Time.y*0.1,IN.uv_MainTex2.y));
			float4 red = tex2D (_MainTex,float2(IN.uv_MainTex.x+_Time.y*0.2+disto.x*0.2, IN.uv_MainTex.y * waterHeight));
			float4 white = tex2D (_MainTex3,float2(IN.uv_MainTex.x+_Time.y*0.2+disto.x*0.2, IN.uv_MainTex.y + waterHeight));
		 
			o.Emission = red.rgb*1.5;
			o.Alpha = red.a;
		}
		ENDCG

		//2pass
	    CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alphatest:_Cutoff addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;

		float _WaterHeight;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
		};
		void surf (Input IN, inout SurfaceOutputStandard o) {
			float4 disto = tex2D (_MainTex2, float2(IN.uv_MainTex2.x+_Time.y*0.1,IN.uv_MainTex2.y));
			float4 white = tex2D (_MainTex3,float2(IN.uv_MainTex.x+_Time.y*0.2+disto.x*0.2, IN.uv_MainTex.y + clamp(1 - _WaterHeight, 0.33, 0.95)));
		 
			o.Emission = white.rgb*1.5;
			o.Alpha = white.a;
		}

		ENDCG
	
	}
	FallBack "Transparent/Cutout/Diffuse"
}
