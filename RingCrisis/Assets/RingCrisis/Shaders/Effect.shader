Shader "RingCrisis/Effect"
{
    Properties
    {
        [Enum(BlendMode)]
        _BlendSrc("Blend Src", Float) = 5 // SrcAlpha

        [Enum(BlendMode)]
        _BlendDst("Blend Dst", Float) = 10 // OneMinusSrcAlpha

        [Enum(BlendOp)]
        _BlendOp("Blend Op", Float) = 0 // Add

        _BaseColor("Base Color", Color) = (0, 0, 0, 0)

        [ToggleUI]
        _PremultiplyAlpha("Premultiply Alpha", Float) = 0

        _MainTex("Texture", 2D) = "white" {}

        _MainTexScrollU("Main Texture Scroll (U)", Float) = 0
        _MainTexScrollV("Main Texture Scroll (V)", Float) = 0

        _TintColor("Tint Color", Color) = (1, 1, 1, 1)

        _DissolveTex("Dissolve Texture", 2D) = "white" {}
        _DissolveLevel("Dissolve Level", Range(0, 1)) = 0
        _DissolveEdge("Dissolve Edge", Range(0.0001, 1)) = 0.5
        _DissolveEdgeColor("Dissolve Edge Color", Color) = (1, 1, 1, 1)
        _DissolveTexScrollU("Dissolve Texture Scroll (U)", Float) = 0
        _DissolveTexScrollV("Dissolve Texture Scroll (V)", Float) = 0
        [ToggleUI]
        _DissolveLevelFromCustomData("Dissolve Level From Custom Data (TEXCOORD1.x)", Float) = 0

        _CameraOffset("Camera Offset", Float) = 0

        [Enum(CompareFunction)]
        _ZTest("Z Test", Float) = 4 // LEqual
    }
    SubShader
    {
        Tags
        {
            "RenderType"      = "Transparent"
            "Queue"           = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType"     = "Plane"
        }

        Blend   [_BlendSrc] [_BlendDst]
        BlendOp [_BlendOp]
        Cull    Off
        ZWrite  Off
        ZTest   [_ZTest]

        Pass
        {
            HLSLPROGRAM
            #pragma target 3.0
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex  : POSITION;
                float4 color   : COLOR;
                float2 uv      : TEXCOORD0;
                float4 custom1 : TEXCOORD1; // x:DissolveLevel
            };

            struct v2f
            {
                float4 vertex  : SV_POSITION;
                float4 color   : COLOR;
                float4 uv      : TEXCOORD0;
                float4 custom1 : TEXCOORD1; // x:DissolveLevel
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _DissolveTex;
            float4 _DissolveTex_ST;

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            half  _PremultiplyAlpha;
            half4 _TintColor;
            half  _DissolveLevel;
            half  _DissolveEdge;
            half4 _DissolveEdgeColor;
            half  _MainTexScrollU;
            half  _MainTexScrollV;
            half  _DissolveTexScrollU;
            half  _DissolveTexScrollV;
            half  _DissolveLevelFromCustomData;
            half  _CameraOffset;
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
#ifdef UNITY_REVERSED_Z
                o.vertex.z += _CameraOffset;
#else
                o.vertex.z -= _CameraOffset;
#endif
                o.uv.xy = TRANSFORM_TEX(v.uv + frac(_Time.y * half2(_MainTexScrollU, _MainTexScrollV)), _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv + frac(_Time.y * half2(_DissolveTexScrollU, _DissolveTexScrollV)), _DissolveTex);
                o.color = v.color;
                o.custom1 = v.custom1;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv.xy) * i.color * _TintColor;
                half dissolve = tex2D(_DissolveTex, i.uv.zw).r;
                half dissolveLevel = lerp(_DissolveLevel, i.custom1.x, _DissolveLevelFromCustomData);
                dissolve = saturate((dissolveLevel - dissolve) * rcp(_DissolveEdge) + dissolveLevel);
                col.a = saturate(col.a - dissolve);
                col.rgb = lerp(_DissolveEdgeColor.rgb, col.rgb, floor(1 - dissolve));
                col.rgb = lerp(col.rgb, lerp(_BaseColor.rgb, col.rgb, col.a), _PremultiplyAlpha);
                return col;
            }
            ENDHLSL
        }
    }
}
