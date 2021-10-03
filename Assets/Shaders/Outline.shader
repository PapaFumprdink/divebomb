Shader "Hidden/PostProcess/Outline"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_GlobalOutlineTexture, sampler_GlobalOutlineTexture);
    float4 _OutlineColor;
    float _OutlineSize;

    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        float4 outlineRef = SAMPLE_TEXTURE2D(_GlobalOutlineTexture, sampler_GlobalOutlineTexture, i.texcoord);

        float4 outline = 0;
        for (uint x = 0; x < _OutlineSize * 2 + 1; x++)
        {
            for (uint y = 0; y < _OutlineSize * 2 + 1; y++)
            {
                float4 outlineSample = SAMPLE_TEXTURE2D(_GlobalOutlineTexture, sampler_GlobalOutlineTexture, i.texcoord + (float2(x, y) - _OutlineSize.xx) / _ScreenParams.xy);

                if (all(outlineSample.rgb > 0) && all(outlineRef.rgb == 0))
                {
                    outline = 1;
                }
            }
        }

        return color + outline * _OutlineColor;
    }

    ENDHLSL
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}
