Shader "Unlit/ValueDisplay"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Font("Font", 2D) = "white" {}
		_Pallet("Pallet",2D) = "white"{}
		_Size ("Size",Vector) = (0,0,0,0)
		_Mods("Mods",Vector) = (0,0,0,0)
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
			#pragma multi_compile_fog
			
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
			sampler2D _Font;
			sampler2D _Pallet;
			float4 _Size;
			float4 _Mods;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			float Font(float v,float2 subUV, float scale, float2 pos) {
				subUV.y = 1 - subUV.y;
				float2 scale2 = float2(scale/16., scale);
				float2 samplePos = subUV + pos;
				float2 offset = (v, 0.5);
				samplePos -= offset;
				samplePos *= scale2;
				samplePos += offset;
				samplePos.x += v+.529*scale2.x;
				samplePos.y = clamp(samplePos.y, 0, 1);
				samplePos.x = clamp(samplePos.x, v, v + (1. / 16.));
				return tex2D(_Font, samplePos);

			}
			float GetSegment(float v, float div) {
				return frac(floor(v*div)*0.1)*.625;
			}
			float GetMemoryAddress(float v, float2 subUV) {
				float a0 = GetSegment(v,1.);
				float a1 = GetSegment(v,0.1);
				float a2 = GetSegment(v,0.01);
				float a3 = GetSegment(v,0.001);
				float a4 = GetSegment(v,0.0001);
				float a5 = GetSegment(v,0.00001);
				float a6 = GetSegment(v,0.000001);
				float 
				font  = Font(a6, subUV, 4.5, float2(-1.8, -0.34));
				font *= Font(a5, subUV, 4.5, float2(-1.95, -0.34));
				font *= Font(a4, subUV, 4.5, float2(-2.05, -0.34));
				font *= Font(a3, subUV, 4.5, float2(-2.15, -0.34));
				font *= Font(a2, subUV, 4.5, float2(-2.3, -0.34));
				font *= Font(a1, subUV, 4.5, float2(-2.4, -0.34));
				font *= Font(a0, subUV, 4.5, float2(-2.5, -0.34));
				return font;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				float rec16 = 1. / 16.;
				fixed4 col = tex2D(_MainTex, i.uv);
				
				//each block size
				float2 texle = i.uv*_Size.xy;
				float2 subUV = frac(texle);
				texle = floor(texle);
                
                
                //high bits
				float font = Font(col.g, subUV, 2.88, float2(-3.52, 0.25));
				//low bits
				font *= Font(col.r, subUV, 2.88, float2(-3.33, 0.25));
				//GetMemoryAddress
				font *= GetMemoryAddress(col.b, subUV);
				
				//Select color, and add some shading.
				float isWhite = tex2D(_Pallet, fixed2(col.g + rec16*0.5, 0.25)).r;
				col = tex2D(_Pallet, fixed2(col.g + rec16*0.5, 0.75));
				col += (length(subUV))*0.15;
				
				return isWhite < 0.1 ?
				    col * font:
				    col + (1 - font);
				
			}
			ENDCG
		}
	}
}
