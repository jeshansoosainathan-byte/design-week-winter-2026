// Shader created with Shader Forge v1.42 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Enhanced by Antoine Guillon / Arkham Development - http://www.arkham-development.com/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.42;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33400,y:32689,varname:node_4795,prsc:2|emission-1700-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32351,y:32648,ptovrint:False,ptlb:Main Texture,ptin:_MainTexture,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6934-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32663,y:32788,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32351,y:32818,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:1621,x:31673,y:32523,ptovrint:False,ptlb:Distortion,ptin:_Distortion,varname:node_1621,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-1032-OUT;n:type:ShaderForge.SFN_Slider,id:6471,x:31526,y:32408,ptovrint:False,ptlb:DIstortion Power,ptin:_DIstortionPower,varname:node_6471,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:6673,x:31848,y:32540,varname:node_6673,prsc:2|A-1621-R,B-6471-OUT;n:type:ShaderForge.SFN_Vector4Property,id:6668,x:30488,y:32635,ptovrint:False,ptlb:Main Texture Scroll,ptin:_MainTextureScroll,varname:node_6668,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Time,id:8225,x:30714,y:32645,varname:node_8225,prsc:2;n:type:ShaderForge.SFN_Append,id:2006,x:30714,y:32473,varname:node_2006,prsc:2|A-6668-Z,B-6668-W;n:type:ShaderForge.SFN_Multiply,id:1789,x:30940,y:32473,varname:node_1789,prsc:2|A-2006-OUT,B-8225-T;n:type:ShaderForge.SFN_TexCoord,id:6962,x:30940,y:32635,varname:node_6962,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:5956,x:31173,y:32473,varname:node_5956,prsc:2|A-6962-UVOUT,B-1789-OUT;n:type:ShaderForge.SFN_Append,id:7873,x:30709,y:32806,varname:node_7873,prsc:2|A-6668-X,B-6668-Y;n:type:ShaderForge.SFN_Multiply,id:2355,x:30935,y:32806,varname:node_2355,prsc:2|A-8225-T,B-7873-OUT;n:type:ShaderForge.SFN_Add,id:6934,x:32021,y:32651,varname:node_6934,prsc:2|A-6673-OUT,B-2165-OUT;n:type:ShaderForge.SFN_Add,id:2165,x:31313,y:32722,varname:node_2165,prsc:2|A-6962-UVOUT,B-2355-OUT;n:type:ShaderForge.SFN_Time,id:1081,x:30713,y:32312,varname:node_1081,prsc:2;n:type:ShaderForge.SFN_Append,id:4067,x:30713,y:32140,varname:node_4067,prsc:2|A-1761-Z,B-1761-W;n:type:ShaderForge.SFN_Multiply,id:8213,x:30940,y:32140,varname:node_8213,prsc:2|A-4067-OUT,B-1081-T;n:type:ShaderForge.SFN_TexCoord,id:5315,x:30940,y:32302,varname:node_5315,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:3618,x:31172,y:32140,varname:node_3618,prsc:2|A-5315-UVOUT,B-8213-OUT;n:type:ShaderForge.SFN_Vector4Property,id:1761,x:30498,y:32140,ptovrint:False,ptlb:Distortion Scroll,ptin:_DistortionScroll,varname:node_1761,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Add,id:1032,x:31387,y:32522,varname:node_1032,prsc:2|A-5956-OUT,B-3618-OUT;n:type:ShaderForge.SFN_Multiply,id:1700,x:32955,y:32788,varname:node_1700,prsc:2|A-7997-RGB,B-2393-OUT,C-3629-OUT;n:type:ShaderForge.SFN_Tex2d,id:7997,x:32351,y:32433,ptovrint:False,ptlb: Mas kTexture,ptin:_MaskTexture,varname:node_7997,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9663-OUT;n:type:ShaderForge.SFN_Slider,id:8962,x:31881,y:33328,ptovrint:False,ptlb:Hue,ptin:_Hue,varname:node_8962,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_HsvToRgb,id:3629,x:32378,y:33383,varname:node_3629,prsc:2|H-8962-OUT,S-5104-OUT,V-9160-OUT;n:type:ShaderForge.SFN_Slider,id:5104,x:31881,y:33440,ptovrint:False,ptlb:Color Saturation,ptin:_ColorSaturation,varname:node_5104,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:9160,x:31881,y:33545,ptovrint:False,ptlb:Power,ptin:_Power,varname:node_9160,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:10;n:type:ShaderForge.SFN_Tex2d,id:5348,x:31898,y:32100,ptovrint:False,ptlb:Mask Distortion,ptin:_MaskDistortion,varname:node_5348,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2999-OUT;n:type:ShaderForge.SFN_Slider,id:9441,x:31741,y:31985,ptovrint:False,ptlb:Mask Distortion Power,ptin:_MaskDistortionPower,varname:node_9441,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.1;n:type:ShaderForge.SFN_Add,id:9663,x:32351,y:32210,varname:node_9663,prsc:2|A-8498-OUT,B-2165-OUT;n:type:ShaderForge.SFN_Multiply,id:8498,x:32115,y:32117,varname:node_8498,prsc:2|A-5348-R,B-9441-OUT;n:type:ShaderForge.SFN_Add,id:2999,x:31556,y:31727,varname:node_2999,prsc:2|A-7042-UVOUT,B-4614-OUT;n:type:ShaderForge.SFN_Multiply,id:4614,x:31319,y:31727,varname:node_4614,prsc:2|A-8740-OUT,B-6548-T;n:type:ShaderForge.SFN_Append,id:8740,x:31097,y:31727,varname:node_8740,prsc:2|A-2611-Z,B-2611-W;n:type:ShaderForge.SFN_Vector4Property,id:2611,x:30851,y:31717,ptovrint:False,ptlb:Mask Distortion Scroll,ptin:_MaskDistortionScroll,varname:node_2611,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Time,id:6548,x:31097,y:31891,varname:node_6548,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:7042,x:31319,y:31881,varname:node_7042,prsc:2,uv:0,uaff:False;proporder:8962-5104-9160-6074-6668-1621-6471-1761-7997-5348-9441-2611;pass:END;sub:END;*/

Shader "RaivoVFX/Simple Flowing Additive" {
    Properties {
        _Hue ("Hue", Range(0, 1)) = 0
        _ColorSaturation ("Color Saturation", Range(0, 1)) = 0
        _Power ("Power", Range(0, 10)) = 1
        _MainTexture ("Main Texture", 2D) = "white" {}
        _MainTextureScroll ("Main Texture Scroll", Vector) = (0,0,0,0)
        _Distortion ("Distortion", 2D) = "white" {}
        _DIstortionPower ("DIstortion Power", Range(0, 1)) = 0
        _DistortionScroll ("Distortion Scroll", Vector) = (0,0,0,0)
        _MaskTexture (" Mas kTexture", 2D) = "white" {}
        _MaskDistortion ("Mask Distortion", 2D) = "white" {}
        _MaskDistortionPower ("Mask Distortion Power", Range(0, 0.1)) = 0
        _MaskDistortionScroll ("Mask Distortion Scroll", Vector) = (0,0,0,0)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #ifndef UNITY_PASS_FORWARDBASE
            #define UNITY_PASS_FORWARDBASE
            #endif //UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu switch vulkan 
            #pragma target 3.0
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform sampler2D _Distortion; uniform float4 _Distortion_ST;
            uniform float _DIstortionPower;
            uniform float4 _MainTextureScroll;
            uniform float4 _DistortionScroll;
            uniform sampler2D _MaskTexture; uniform float4 _MaskTexture_ST;
            uniform float _Hue;
            uniform float _ColorSaturation;
            uniform float _Power;
            uniform sampler2D _MaskDistortion; uniform float4 _MaskDistortion_ST;
            uniform float _MaskDistortionPower;
            uniform float4 _MaskDistortionScroll;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_6548 = _Time;
                float2 node_2999 = (i.uv0+(float2(_MaskDistortionScroll.b,_MaskDistortionScroll.a)*node_6548.g));
                float4 _MaskDistortion_var = tex2D(_MaskDistortion,TRANSFORM_TEX(node_2999, _MaskDistortion));
                float4 node_8225 = _Time;
                float2 node_2165 = (i.uv0+(node_8225.g*float2(_MainTextureScroll.r,_MainTextureScroll.g)));
                float2 node_9663 = ((_MaskDistortion_var.r*_MaskDistortionPower)+node_2165);
                float4 _MaskTexture_var = tex2D(_MaskTexture,TRANSFORM_TEX(node_9663, _MaskTexture));
                float4 node_1081 = _Time;
                float2 node_1032 = ((i.uv0+(float2(_MainTextureScroll.b,_MainTextureScroll.a)*node_8225.g))+(i.uv0+(float2(_DistortionScroll.b,_DistortionScroll.a)*node_1081.g)));
                float4 _Distortion_var = tex2D(_Distortion,TRANSFORM_TEX(node_1032, _Distortion));
                float2 node_6934 = ((_Distortion_var.r*_DIstortionPower)+node_2165);
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(node_6934, _MainTexture));
                float3 emissive = (_MaskTexture_var.rgb*(_MainTexture_var.rgb*i.vertexColor.rgb)*(lerp(float3(1,1,1),saturate(3.0*abs(1.0-2.0*frac(_Hue+float3(0.0,-1.0/3.0,1.0/3.0)))-1),_ColorSaturation)*_Power));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
