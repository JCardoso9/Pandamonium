// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders/XRay"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DisplaceTex("Displacement Texture", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0,0.1)) = 1
		_Color("Tint", Color) = (1,1,1,1)
		_EffectAmount("Effect Amount", Range(0, 1)) = 1.0
	}
		SubShader
	{

		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			fixed4 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color * _Color;
				o.vertex = UnityPixelSnap(o.vertex);
				return o;
			}

			sampler2D _MainTex;
			sampler2D _DisplaceTex;
			float _Magnitude;
			uniform float _EffectAmount;

			float4 frag(v2f i) : SV_Target
			{
				float2 disp = tex2D(_DisplaceTex, float2(i.uv.x + _Time.x, i.uv.y)).xy;
				disp = ((disp * 2) - 1) * _Magnitude;
				

				float4 col = tex2D(_MainTex, i.uv + disp);
				col.rgb = lerp(col.rgb, dot(col.rgb, float3(0.3, 0.59, 0.11)), _EffectAmount);
				col = col * i.color;
				return col;
			}
			ENDCG
		}
	}
}