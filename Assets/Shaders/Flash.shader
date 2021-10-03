Shader "Unlit/Flash"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color1("Color One", Color) = (1, 1, 1, 1)
        _Color2("Color Two", Color) = (0, 0, 0, 1)
        _FlashTime("Flash Time", float) = 0.5
        _FlashSplit("Flash Split", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderQueue"="Transparent"}
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
                float2 uv : TEXCOORD0;
                float4 vertexCol : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 vertexCol : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertexCol = v.vertexCol;
                return o;
            }

            float4 _Color1;
            float4 _Color2;
            float _FlashTime;
            float _FlashSplit;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color1;
                if (((_Time.y % _FlashTime) / _FlashTime) > _FlashSplit)
                {
                    col = _Color2;
                }
                return col * tex2D(_MainTex, i.uv) * i.vertexCol;
            }
            ENDCG
        }
    }
}
