Shader "RingCrisis/Model"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _MainTexBlend("Main Texture Blend", Range(0, 1)) = 1
        _Color("Color", Color) = (1, 1, 1, 1)
        _ShadeColor("Shade Color", Color) = (0, 0, 0, 1)
        _SpecularSharpness("Specular Sharpness", Float) = 1
        _SpecularColor("Specular Color", Color) = (1, 1, 1, 1)
        _SpecularIntensity("Specular Intensity", Float) = 1

        _RimColor("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower("Rim Power", Float) = 10
        _RimIntensity("Rim Intensity", Float) = 1

        _UVScroll("UV Scroll", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Name "MAIN"
        Tags
        {
            "RenderType" = "Opaque"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex    : SV_POSITION;
                float3 normalWS  : NORMAL;
                float2 uv        : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
                float3 halfDirWS : TEXCOORD2;
            };

            sampler2D _MainTex;

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half   _MainTexBlend;
            half4  _Color;
            half4  _ShadeColor;
            half   _SpecularSharpness;
            half4  _SpecularColor;
            half   _SpecularIntensity;
            half4  _RimColor;
            half   _RimPower;
            half   _RimIntensity;
            half4  _UVScroll;
            CBUFFER_END

            v2f vert(appdata v)
            {
                float3 posWS = TransformObjectToWorld(v.vertex.xyz);

                v2f o;
                o.vertex = TransformWorldToHClip(posWS);
                o.uv = TRANSFORM_TEX((v.uv + _UVScroll.xy * _Time.y), _MainTex);
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.viewDirWS = normalize(_WorldSpaceCameraPos - posWS);
                o.halfDirWS = normalize(o.viewDirWS + normalize(_MainLightPosition.xyz));
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float3 normalWS = normalize(i.normalWS);
                float3 viewDirWS = normalize(i.viewDirWS);
                float3 halfDirWS = normalize(i.halfDirWS);

                // テクスチャカラーをサンプリング
                float4 texColor = tex2D(_MainTex, i.uv);

                // ディフューズ（Half-Lambertモデル）
                float diffuseStrength = 0.5 * dot(normalWS, _MainLightPosition.xyz) + 0.5;
                diffuseStrength *= diffuseStrength;
                float3 diffuseColor = lerp(_ShadeColor.rgb, _MainLightColor.a * _MainLightColor.rgb, diffuseStrength);

                // スペキュラ（Blinn-Phongモデル）
                float3 specularLight = _SpecularIntensity * _SpecularColor.rgb * pow(saturate(dot(halfDirWS, normalWS)), _SpecularSharpness);

                // リムライティング
                float3 rimLight = _RimColor.rgb * pow(max(0, _RimIntensity * (1 - max(0, dot(normalWS, viewDirWS)))), _RimPower);

                // 最終的な乗算色を計算
                float3 finalColor = _Color.a * _Color.rgb * diffuseColor * lerp(float3(1, 1, 1), texColor.rgb, _MainTexBlend) + specularLight + rimLight;

                return float4(saturate(finalColor), 1);
            }
            ENDHLSL
        }
        Pass
        {
            // 他のオブジェクトに影を落とすためのパス
            // このオブジェクトの形状だけ分かればいい

            Name "SHADOWCASTER"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half   _MainTexBlend;
            half4  _Color;
            half4  _ShadeColor;
            half   _SpecularSharpness;
            half4  _SpecularColor;
            half   _SpecularIntensity;
            half4  _RimColor;
            half   _RimPower;
            half   _RimIntensity;
            half4  _UVScroll;
            CBUFFER_END

            v2f vert(appdata v)
            {
                float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                float3 normalWS = TransformObjectToWorldNormal(v.normal);

                v2f o;
                o.vertex = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
}
