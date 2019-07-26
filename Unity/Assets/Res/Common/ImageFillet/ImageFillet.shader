// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/ImageFillet"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0

        _RoundedRadius("Rounded Radius", Range(0, 256)) = 64
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

        Pass
        {
            CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"
#include "UnityUI.cginc"

#pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
float4 vertex   :
                POSITION;
float4 color    :
                COLOR;
float2 texcoord :
                TEXCOORD0;
            };

            struct v2f
            {
float4 vertex   :
                SV_POSITION;
fixed4 color :
                COLOR;
half2 texcoord  :
                TEXCOORD0;
float4 worldPosition :
                TEXCOORD1;
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            float _RoundedRadius;

            float4 _MainTex_TexelSize;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.worldPosition = IN.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = IN.texcoord;

#ifdef UNITY_HALF_TEXEL_OFFSET
                OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
#endif

                OUT.color = IN.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

#ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
#endif

                float width = _MainTex_TexelSize.z;
                float height = _MainTex_TexelSize.w;

                float x = IN.texcoord.x * width;
                float y = IN.texcoord.y * height;

                float r = _RoundedRadius;

                //左下角
                if (x < r && y < r)
                {
                    if ((x - r) * (x - r) + (y - r) * (y - r) > r * r)
                        color.a = 0;
                }

                //左上角
                if (x < r && y > (height - r))
                {
                    if ((x - r) * (x - r) + (y - (height - r)) * (y - (height - r)) > r * r)
                        color.a = 0;
                }

                //右下角
                if (x > (width - r) && y < r)
                {
                    if ((x - (width - r)) * (x - (width - r)) + (y - r) * (y - r) > r * r)
                        color.a = 0;
                }

                //右上角
                if (x > (width - r) && y > (height - r))
                {
                    if ((x - (width - r)) * (x - (width - r)) + (y - (height - r)) * (y - (height - r)) > r * r)
                        color.a = 0;
                }

                return color;
            }
            ENDCG
        }
    }
}