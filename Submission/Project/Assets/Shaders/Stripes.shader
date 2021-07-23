Shader "Unlit/Stripes"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _BandWidth("Band Width", float) = 0.01
        _BandCutoff("Band Ratio", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 worldpos : TEXCOORD0;
            };

            half4 _Color;
            float _BandWidth;
            float _BandCutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldpos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float sampleSpace = abs(i.worldpos.y);
                float band = ((sampleSpace + 1) % _BandWidth) / _BandWidth > _BandCutoff;
                return _Color * float4(1, 1, 1, band);
            }
            ENDCG
        }
    }
}
