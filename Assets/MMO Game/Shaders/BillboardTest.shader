﻿Shader "Unlit/BillboardTest"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "DisableBatching"="True" }
		LOD 100

		Pass
		{
      //Cull front

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
			
			v2f vert (appdata v)
			{
				v2f o;

        float3 pos = (v.vertex.x * UNITY_MATRIX_V[0] + v.vertex.y * float4(0.0, 1.0, 0.0, 0.0)).xyz;
        o.vertex = float4(pos, 1.0);

        o.vertex = UnityObjectToClipPos(o.vertex);
        

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
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

    Pass
    {
        Tags {"LightMode"="ShadowCaster"}
        /*
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_shadowcaster
        #include "UnityCG.cginc"

        struct v2f {
          V2F_SHADOW_CASTER;
        };

       v2f vert(appdata_base v)
       {
         v2f o;
         TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
         return o;
       }

       float4 frag(v2f i) : SV_Target
       {
         SHADOW_CASTER_FRAGMENT(i);
       }
       ENDCG
       */
    }
	}
}
