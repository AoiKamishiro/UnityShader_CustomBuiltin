/*
 * Copyright (c) 2020 AoiKamishiro
 * 
 * This code is provided under the MIT license.
 *
 * This program uses the following code, which is provided under the MIT License.
 * https://download.unity3d.com/download_unity/008688490035/builtin_shaders-2018.4.20f1.zip?_ga=2.171325672.957521966.1599549120-262519615.1592172043
 * 
 */
 
 Shader "AKUnlit"
{
    Properties
    {
        _Color ("Albedo", Color) = (1, 1, 1, 1)
        _MainTex ("Albedo", 2D) = "white" { }
        _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
        [HideInInspector] _ModeU ("__mode", Float) = 0.0
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", float) = 2
        [HideInInspector] [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("__src", Float) = 1.0
        [HideInInspector] [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("__dst", Float) = 0.0
        [HideInInspector] [Enum(Off, 0, On, 1)] _ZWrite ("__zw", Float) = 1.0
        [HideInInspector] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("__zt", Float) = 4.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "IgnoreProjector" = "True" }
        LOD 100
        Cull [_Cull]
        Blend [_SrcBlend] [_DstBlend]
        ZWrite [_ZWrite]
        ZTest [_ZTest]
        Lighting Off

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma shader_feature _ _ALPHATEST_ON _ALPHAPREMULTIPLY_ON
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex: POSITION;
                float2 texcoord: TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex: SV_POSITION;
                float2 texcoord: TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Cutoff;
            float4 _MainTex_ST;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
            fixed4 frag(v2f i): SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord.xy).rgba * _Color.rgba;
                #ifdef _ALPHATEST_ON
                    clip(col.a - _Cutoff);
                    UNITY_APPLY_FOG(i.fogCoord, col);
                #elif _ALPHAPREMULTIPLY_ON
                    UNITY_APPLY_FOG(i.fogCoord, col);
                #else
                    UNITY_APPLY_FOG(i.fogCoord, col);
                    UNITY_OPAQUE_ALPHA(col.a);
                #endif
                return col;
            }
            ENDCG
            
        }
    }
    CustomEditor "AKSBuiltin.AKSUnlitShaderGUI"
}
