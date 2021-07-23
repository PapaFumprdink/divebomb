Shader "Hidden/PostProcess/Distortion"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_Distortion, sampler_Distortion);
    float _Strength;

    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float4 distortion = SAMPLE_TEXTURE2D(_Distortion, sampler_Distortion, i.texcoord);
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + (distortion.xy * 2 - 1) * _Strength);

        return color;
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
