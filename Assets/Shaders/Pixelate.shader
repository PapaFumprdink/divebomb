Shader "Hidden/PostProcess/Pixelate"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    float _PixelScale;

    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float2 pixelScale = unity_OrthoParams.xy * _PixelScale;
        float2 uv = round(i.texcoord * pixelScale) / pixelScale;
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
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
