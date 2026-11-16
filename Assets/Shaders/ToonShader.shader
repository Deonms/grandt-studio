Shader "URP/ToonShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _Steps ("Color Steps", Range(1, 5)) = 3
        _AmbientStrength ("Ambient Strength", Range(0, 1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Steps;
                float _AmbientStrength;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.worldNormal = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);

                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);

                float cosineAngle = dot(normal, lightDir);
                cosineAngle = max(cosineAngle, 0.0);

                // Cel-shading met een minimum ondergrens (AmbientStrength) zodat het niet zwart wordt
                float shaded = floor(cosineAngle * _Steps) / _Steps;
                shaded = max(shaded, _AmbientStrength);

                float3 finalColor = _BaseColor.rgb * shaded;

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}