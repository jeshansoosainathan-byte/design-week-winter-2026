Shader "Custom/Simple Flowing Unlit 2D"
{
    Properties
    {
        [Header(Color)]
        _Hue ("Hue", Range(0, 1)) = 0
        _ColorSaturation ("Color Saturation", Range(0, 1)) = 0
        _Power ("Power", Range(0, 25)) = 1
        _Fade ("Fade", Range(0, 1)) = 1
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Main Texture)]
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _FlowTexture ("Flow Texture", 2D) = "white" {}
        _FlowTextureScroll ("Flow Texture Scroll (XY = main, ZW = secondary)", Vector) = (0,0,0,0)

        [Header(Distortion)]
        _Distortion ("Distortion", 2D) = "white" {}
        _DistortionPower ("Distortion Power", Range(0, 1)) = 0
        _DistortionScroll ("Distortion Scroll (XY = unused, ZW = scroll speed)", Vector) = (0,0,0,0)

        [Header(Mask)]
        _MaskTexture ("Mask Texture", 2D) = "white" {}
        _MaskDistortion ("Mask Distortion", 2D) = "white" {}
        _MaskDistortionPower ("Mask Distortion Power", Range(0, 0.1)) = 0
        _MaskDistortionScroll ("Mask Distortion Scroll (XY = unused, ZW = scroll speed)", Vector) = (0,0,0,0)

        // Sprite renderer stencil support
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderPipeline" = "UniversalPipeline"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Name "Unlit2DFlowing"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);            SAMPLER(sampler_MainTex);
            TEXTURE2D(_FlowTexture);        SAMPLER(sampler_FlowTexture);
            TEXTURE2D(_Distortion);         SAMPLER(sampler_Distortion);
            TEXTURE2D(_MaskTexture);        SAMPLER(sampler_MaskTexture);
            TEXTURE2D(_MaskDistortion);     SAMPLER(sampler_MaskDistortion);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _FlowTexture_ST;
                float4 _Distortion_ST;
                float4 _MaskTexture_ST;
                float4 _MaskDistortion_ST;
                float4 _FlowTextureScroll;
                float4 _DistortionScroll;
                float4 _MaskDistortionScroll;
                float4 _Color;
                float _DistortionPower;
                float _MaskDistortionPower;
                float _Hue;
                float _ColorSaturation;
                float _Power;
                float _Fade;
            CBUFFER_END

            float3 HsvToRgb(float h, float s, float v)
            {
                float3 offset = float3(0.0, -1.0 / 3.0, 1.0 / 3.0);
                float3 rgb = saturate(3.0 * abs(1.0 - 2.0 * frac(h + offset)) - 1.0);
                return lerp(float3(1, 1, 1), rgb, s) * v;
            }

            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float t = _Time.y;

                // --- Sprite base texture (from Sprite Renderer) ---
                float4 spriteColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, TRANSFORM_TEX(i.uv, _MainTex));

                // --- Mask Distortion UV ---
                float2 maskDistUV = i.uv + float2(_MaskDistortionScroll.z, _MaskDistortionScroll.w) * t;
                float maskDistSample = SAMPLE_TEXTURE2D(_MaskDistortion, sampler_MaskDistortion, TRANSFORM_TEX(maskDistUV, _MaskDistortion)).r;

                // --- Main scrolling UV ---
                float2 mainScrollUV = i.uv + t * float2(_FlowTextureScroll.x, _FlowTextureScroll.y);

                // --- Mask UV ---
                float2 maskUV = maskDistSample * _MaskDistortionPower + mainScrollUV;
                float4 maskColor = SAMPLE_TEXTURE2D(_MaskTexture, sampler_MaskTexture, TRANSFORM_TEX(maskUV, _MaskTexture));

                // --- Distortion UV ---
                float2 secondaryScrollUV = i.uv + float2(_FlowTextureScroll.z, _FlowTextureScroll.w) * t;
                float2 distScrollUV = i.uv + float2(_DistortionScroll.z, _DistortionScroll.w) * t;
                float2 distCombinedUV = secondaryScrollUV + distScrollUV;
                float distSample = SAMPLE_TEXTURE2D(_Distortion, sampler_Distortion, TRANSFORM_TEX(distCombinedUV, _Distortion)).r;

                // --- Flow Texture UV ---
                float2 flowUV = distSample * _DistortionPower + mainScrollUV;
                float4 flowColor = SAMPLE_TEXTURE2D(_FlowTexture, sampler_FlowTexture, TRANSFORM_TEX(flowUV, _FlowTexture));

                // --- HSV color tint ---
                float3 hsvColor = HsvToRgb(_Hue, _ColorSaturation, _Power);

                // --- Final composite ---
                // Sprite alpha controls the overall shape
                // Mask alpha controls which parts of the flow effect are visible
                float3 finalRGB = maskColor.rgb * (flowColor.rgb * i.color.rgb) * hsvColor;
                float finalAlpha = spriteColor.a * flowColor.a * maskColor.a * i.color.a * _Fade;

                return half4(finalRGB, finalAlpha);
            }
            ENDHLSL
        }

        // Second pass so it also renders in the Universal2D renderer
        Pass
        {
            Name "Universal2DPass"
            Tags { "LightMode" = "Universal2D" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);            SAMPLER(sampler_MainTex);
            TEXTURE2D(_FlowTexture);        SAMPLER(sampler_FlowTexture);
            TEXTURE2D(_Distortion);         SAMPLER(sampler_Distortion);
            TEXTURE2D(_MaskTexture);        SAMPLER(sampler_MaskTexture);
            TEXTURE2D(_MaskDistortion);     SAMPLER(sampler_MaskDistortion);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _FlowTexture_ST;
                float4 _Distortion_ST;
                float4 _MaskTexture_ST;
                float4 _MaskDistortion_ST;
                float4 _FlowTextureScroll;
                float4 _DistortionScroll;
                float4 _MaskDistortionScroll;
                float4 _Color;
                float _DistortionPower;
                float _MaskDistortionPower;
                float _Hue;
                float _ColorSaturation;
                float _Power;
                float _Fade;
            CBUFFER_END

            float3 HsvToRgb(float h, float s, float v)
            {
                float3 offset = float3(0.0, -1.0 / 3.0, 1.0 / 3.0);
                float3 rgb = saturate(3.0 * abs(1.0 - 2.0 * frac(h + offset)) - 1.0);
                return lerp(float3(1, 1, 1), rgb, s) * v;
            }

            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float t = _Time.y;

                float4 spriteColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, TRANSFORM_TEX(i.uv, _MainTex));

                float2 maskDistUV = i.uv + float2(_MaskDistortionScroll.z, _MaskDistortionScroll.w) * t;
                float maskDistSample = SAMPLE_TEXTURE2D(_MaskDistortion, sampler_MaskDistortion, TRANSFORM_TEX(maskDistUV, _MaskDistortion)).r;

                float2 mainScrollUV = i.uv + t * float2(_FlowTextureScroll.x, _FlowTextureScroll.y);

                float2 maskUV = maskDistSample * _MaskDistortionPower + mainScrollUV;
                float4 maskColor = SAMPLE_TEXTURE2D(_MaskTexture, sampler_MaskTexture, TRANSFORM_TEX(maskUV, _MaskTexture));

                float2 secondaryScrollUV = i.uv + float2(_FlowTextureScroll.z, _FlowTextureScroll.w) * t;
                float2 distScrollUV = i.uv + float2(_DistortionScroll.z, _DistortionScroll.w) * t;
                float2 distCombinedUV = secondaryScrollUV + distScrollUV;
                float distSample = SAMPLE_TEXTURE2D(_Distortion, sampler_Distortion, TRANSFORM_TEX(distCombinedUV, _Distortion)).r;

                float2 flowUV = distSample * _DistortionPower + mainScrollUV;
                float4 flowColor = SAMPLE_TEXTURE2D(_FlowTexture, sampler_FlowTexture, TRANSFORM_TEX(flowUV, _FlowTexture));

                float3 hsvColor = HsvToRgb(_Hue, _ColorSaturation, _Power);

                float3 finalRGB = maskColor.rgb * (flowColor.rgb * i.color.rgb) * hsvColor;
                float finalAlpha = spriteColor.a * flowColor.a * maskColor.a * i.color.a * _Fade;

                return half4(finalRGB, finalAlpha);
            }
            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}
