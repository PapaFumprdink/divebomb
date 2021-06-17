Shader "Unlit/Water"
{
    Properties
    {
        //_Noise ("Noise Reference Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float3 worldPos : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float value = 1;
                value *= sin(i.worldPos.y * 8   + _Time.x * -6) * 0.5 + 0.5;
                value *= sin(i.worldPos.y * 4   + _Time.x * -7) * 0.5 + 0.5;
                value *= sin(i.worldPos.y * 2   + _Time.x * -3) * 0.5 + 0.5;
                value *= sin(i.worldPos.y * 1   + _Time.x *  2)  * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 2   + _Time.x * -8)  * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 4   + _Time.x *  6)  * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 8   + _Time.x * -5)   * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 16  + _Time.x * -6)   * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 32  + _Time.x * -1)   * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 64  + _Time.x * -4)   * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 128 + _Time.x * -5) * 0.5 + 0.5;

                value *= sin(i.worldPos.x / 80  + _Time.x * -2)  * 0.5 + 0.5;
                value *= sin(i.worldPos.x / 40  + _Time.x *  4)  * 0.5 + 0.5;
                value *= sin(i.worldPos.x / 20  + _Time.x * -8)  * 0.5 + 0.5;
                value *= sin(i.worldPos.x / 10  + _Time.x *  16) * 0.5 + 0.5;

                value = pow(value, 0.1) * 0.9;

                float4 col = float4(1, 1, 1, 1);
                col.rgb = value.rrr;
                return col;
            }
            ENDCG
        }
    }
}
