Shader "Custom/fire2" {
	Properties {
		_Color("MainColor",color) =(1,1,1,1) 
		_MainTex ("빨간색", 2D) = "white" {}
		_MainTex2 ("일렁일렁", 2D) = "white" {}
		[HideinInspector]_Cutoff("Cutoff",float) = 0.5

	}
	SubShader {
		Tags { "RenderType"="TransparentCutout" " Queue" = "AlphaTest"}
		LOD 200
		cull off
	
	

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alphatest:_Cutoff addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
		    fixed4 disto = tex2D (_MainTex2, float2(IN.uv_MainTex2.x+_Time.y*0.1,IN.uv_MainTex2.y));
			fixed4 red = tex2D (_MainTex,float2(IN.uv_MainTex.x+_Time.y*0.2+disto.x*0.2, IN.uv_MainTex.y));


			o.Emission = red.rgb*1.5;

			o.Alpha = red.a;
		}
		ENDCG
	}
	FallBack "Transparent/Cutout/Diffuse"
}
