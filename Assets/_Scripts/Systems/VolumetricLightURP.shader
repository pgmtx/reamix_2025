Shader "Custom/VolumetricLightURP"
{
    Properties
    {
        _Density ("Density", Float) = 0.05
        _Steps ("Steps", Int) = 32
        _MaxDistance ("Max Distance", Float) = 20
        _LightDir ("Light Direction", Vector) = (0, -1, 0, 0)
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            Name "VolumetricLight"
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D_X(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            float _Density;
            int _Steps;
            float _MaxDistance;
            float3 _LightDir;

            Varyings Vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            float Frag (Varyings i) : SV_Target
            {
                float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv).r;
                float linearDepth = LinearEyeDepth(depth, _ZBufferParams);

                float3 rayOrigin = _WorldSpaceCameraPos;
                float3 rayDir = normalize(ComputeWorldSpacePosition(i.uv, depth, UNITY_MATRIX_I_VP) - rayOrigin);

                float stepSize = _MaxDistance / _Steps;
                float3 pos = rayOrigin;
                float lightAccum = 0;

                for (int s = 0; s < _Steps; s++)
                {
                    pos += rayDir * stepSize;
                    float dist = distance(rayOrigin, pos);
                    if (dist > linearDepth) break;

                    float NdotL = saturate(dot(-_LightDir, rayDir));
                    lightAccum += NdotL * _Density * stepSize;
                }

                return lightAccum;
            }
            ENDHLSL
        }
    }
}
