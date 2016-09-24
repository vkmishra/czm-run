﻿Shader "Unlit/Curved"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_QOffset("Offset", Vector) = (8,1,0,0)
		_Dist("Distance", Float) = 100.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _QOffset;
			float _Dist;
			
			v2f vert (appdata v)
			{
				v2f o;
				//o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);
				float zOff = vPos.z / _Dist;
				vPos += _QOffset*zOff*zOff;
				o.vertex = mul(UNITY_MATRIX_P, vPos);
				//o.uv = mul(UNITY_MATRIX_TEXTURE0, v.uv);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
