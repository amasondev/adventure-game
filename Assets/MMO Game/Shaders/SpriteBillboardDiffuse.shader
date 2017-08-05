// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Diffuse - Billboard"
{
  Properties
  {
    [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
  _Color("Tint", Color) = (1,1,1,1)
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
    [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
  }

    SubShader
  {
    Tags
  {
    "Queue" = "AlphaTest"
    "IgnoreProjector" = "True"
    "RenderType" = "TransparentCutout"
    "PreviewType" = "Plane"
    "CanUseSpriteAtlas" = "True"
    "DisableBatching" = "True"
  }

    Cull Off
    //Lighting Off
    //ZWrite Off
    //Blend One OneMinusSrcAlpha

    CGPROGRAM
#pragma surface surf Lambert vertex:vert addshadow alphatest:_Cutoff
#pragma multi_compile _ PIXELSNAP_ON
#include "UnitySprites.cginc"
#include "UnityCG.cginc"

    struct Input
  {
    float2 uv_MainTex;
    fixed4 color;
  };

  void vert(inout appdata_full v, out Input o)
  {
    //v.normal = normalize(UnityObjectToClipPos(v.normal));
    v.vertex.xy *= _Flip.xy;
    float3 pos = (v.vertex.x * UNITY_MATRIX_V[0] + v.vertex.y * float4(0.0, 1.0, 0.0, 0.0)).xyz;
    v.vertex = float4(pos, 1.0);

#if defined(PIXELSNAP_ON)
    v.vertex = UnityPixelSnap(v.vertex);
#endif

    UNITY_INITIALIZE_OUTPUT(Input, o);
    o.color = v.color * _Color * _RendererColor;
    //o.color.xyz = v.normal * 0.5 + 0.5;
  }

  void surf(Input IN, inout SurfaceOutput o)
  {
    fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
    o.Albedo = c.rgb;
    o.Alpha = c.a;
  }
  ENDCG
  }

    Fallback "Sprites/Diffuse"
}
