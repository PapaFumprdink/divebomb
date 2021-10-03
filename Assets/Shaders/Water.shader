Shader "Unlit/Water"
{
    Properties
    {
        _ReflectionTex("Reflection Texture", 2D) = "black" {}
        _ReflectionPos("Reflection Position", Vector) = (1, 1, 1, 1)
        _ReflectionSize("Reflection Scale", Vector) = (1, 1, 1, 1)

        _ImpactTimeScale("Impact Time Scale", float) = 1
        _ImpactDisapationDuration("Impact Disapation Duration", float) = 1
        _ImpactDisapationAmplitude("Impact Disapation Amplitude", float) = 1
        _ImpactDisapationFrequency("Impact Disapation Frequency", float) = 1
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
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float4 _Impacts[64];

            float _ImpactDisapationDuration;
            float _ImpactTimeScale;
            float _ImpactDisapationAmplitude;
            float _ImpactDisapationFrequency;

            v2f vert (appdata v)
            {
                v2f o;

                float pi = 3.14159265;

                float offset = 0;
                for (uint i = 0; i < 64; i++)
                {
                    float time = _Impacts[i].z * _ImpactTimeScale;

                    float distance = (v.vertex.x - _Impacts[i].x / 400) - time / 400;
                    float value = -cos(distance * _ImpactDisapationFrequency) * (distance > -pi / 2 / _ImpactDisapationFrequency) * (distance < pi * 1.5 / _ImpactDisapationFrequency);

                    distance = (v.vertex.x - _Impacts[i].x / 400) + time / 400;
                    value += -cos(distance * _ImpactDisapationFrequency) * (distance > -pi * 1.5 / _ImpactDisapationFrequency) * (distance < pi / 2 / _ImpactDisapationFrequency);

                    value *= _ImpactDisapationAmplitude / 200;
                    value *= pow(2, -(time, time));
                    offset += value * _Impacts[i].w;
                }
                v.vertex.y += offset;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            sampler2D _ReflectionTex;
            float2 _ReflectionPos;
            float2 _ReflectionSize;

            float GetDistortionValue(float2 worldPos)
            {
                float value = 1;
                value *= sin(worldPos.y * 8 + _Time.x * -6) * 0.5 + 0.5;
                value *= sin(worldPos.y * 4 + _Time.x * -7) * 0.5 + 0.5;
                value *= sin(worldPos.y * 2 + _Time.x * -3) * 0.5 + 0.5;
                value *= sin(worldPos.y * 1 + _Time.x * 2) * 0.5 + 0.5;
                value *= sin(worldPos.y / 2 + _Time.x * -8) * 0.5 + 0.5;
                value *= sin(worldPos.y / 4 + _Time.x * 6) * 0.5 + 0.5;
                value *= sin(worldPos.y / 8 + _Time.x * -5) * 0.5 + 0.5;
                value *= sin(worldPos.y / 16 + _Time.x * -6) * 0.5 + 0.5;
                value *= sin(worldPos.y / 32 + _Time.x * -1) * 0.5 + 0.5;

                value *= sin(worldPos.x / 16 + _Time.x * -2) * 0.5 + 0.5;
                value *= sin(worldPos.x / 8 + _Time.x * 4) * 0.5 + 0.5;
                value *= sin(worldPos.x / 4 + _Time.x * -8) * 0.5 + 0.5;
                value *= sin(worldPos.x / 2 + _Time.x * 16) * 0.5 + 0.5;

                return value;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float value = GetDistortionValue(i.worldPos);

                value = pow(value, 0.1) * .6;

                float2 reflectionUV = (i.worldPos.xy - _ReflectionPos + float2(_ReflectionSize.x, -_ReflectionSize.y * 1.5)) / _ReflectionSize / float2(2, -2);
                float4 reflection = tex2D(_ReflectionTex, reflectionUV + float2(sin((i.worldPos.y * 8) + _Time.y * 5) * 0.001, 0));
                value = clamp(value, 0, 1);
                value = clamp(reflection + value, 0, 1);

                float4 col = float4(1, 1, 1, 1);
                col.rgb = value.rrr;

                float surfaceColor = i.uv.y * 200 - 199;

                return col + surfaceColor;
            }
            ENDCG
        }
    }
}
