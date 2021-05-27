Shader "Hidden/PostProcess/Palette"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_Palette, sampler_Palette);
    TEXTURE2D_SAMPLER2D(_OldPalette, sampler_OldPalette);
    TEXTURE2D_SAMPLER2D(_BlendMask, sampler_BlendMask);
    float _Blend;

    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        float sampleSpace = (color.r + color.g + color.b) / 3;

        float4 paletteColor = SAMPLE_TEXTURE2D(_Palette, sampler_Palette, float2(sampleSpace, 0.5));
        float4 oldPaletteColor = SAMPLE_TEXTURE2D(_OldPalette, sampler_OldPalette, float2(sampleSpace, 0.5));
        float4 paletteBlend = SAMPLE_TEXTURE2D(_BlendMask, sampler_BlendMask, i.texcoord);
        float4 finalColor = lerp(oldPaletteColor, paletteColor, paletteBlend < _Blend);

        return finalColor;
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
