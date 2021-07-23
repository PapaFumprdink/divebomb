Shader "Unlit/Water"
{
    Properties
    {
        _ReflectionTex ("Reflection Texture", 2D) = "black" {}
        _ReflectionPos ("Reflection Position", Vector) = (1, 1, 1, 1)
        _ReflectionSize ("Reflection Scale", Vector) = (1, 1, 1, 1)
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

            sampler2D _ReflectionTex;
            float2 _ReflectionPos;
            float2 _ReflectionSize;

            fixed4 frag(v2f i) : SV_Target
            {
                float value = 1;
                value *= sin(i.worldPos.y * 8   + _Time.x * -6) * 0.5 + 0.5;
                value *= sin(i.worldPos.y * 4   + _Time.x * -7) * 0.5 + 0.5;
                value *= sin(i.worldPos.y * 2   + _Time.x * -3) * 0.5 + 0.5;
                value *= sin(i.worldPos.y * 1   + _Time.x *  2) * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 2   + _Time.x * -8) * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 4   + _Time.x *  6) * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 8   + _Time.x * -5) * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 16  + _Time.x * -6) * 0.5 + 0.5;
                value *= sin(i.worldPos.y / 32  + _Time.x * -1) * 0.5 + 0.5;

                value *= sin(i.worldPos.x / 16 + _Time.x * -2)  * 0.5 + 0.5;
                value *= sin(i.worldPos.x / 8  + _Time.x *  4)  * 0.5 + 0.5;
                value *= sin(i.worldPos.x / 4  + _Time.x * -8)  * 0.5 + 0.5;
                value *= sin(i.worldPos.x / 2  + _Time.x *  16) * 0.5 + 0.5;

                value = pow(value, 0.1) * .6;

                float2 reflectionUV = (i.worldPos.xy - _ReflectionPos + float2(_ReflectionSize.x, -_ReflectionSize.y * 1.5)) / _ReflectionSize / float2(2, -2);
                float4 reflection = tex2D(_ReflectionTex, reflectionUV + float2(sin((i.worldPos.y * 8) + _Time.y * 5) * 0.001, 0));
                value = clamp(value, 0, 1);
                value = clamp(reflection + value, 0, 1);

                float4 col = float4(1, 1, 1, 1);
                col.rgb = value.rrr;
                return col;
            }
            ENDCG
        }
    }
}
