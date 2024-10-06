Shader "Custom/CrackShader"
{
    Properties
    {
        _MainTex ("Wall Texture", 2D) = "white" {}
        _CrackTex ("Crack Texture", 2D) = "white" {}
        _CrackStrength ("Crack Strength", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _CrackTex;
        float _CrackStrength;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_CrackTex;
        };

        half4 LightingStandard(SurfaceOutputStandard s, half3 lightDir, half atten) : SV_Target
        {
            return half4(0,0,0,0); // optional lighting if you want
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the wall texture
            half4 mainTex = tex2D(_MainTex, IN.uv_MainTex);

            // Sample the crack texture and control its strength
            half4 crackTex = tex2D(_CrackTex, IN.uv_CrackTex) * _CrackStrength;

            // Combine the two textures
            o.Albedo = mainTex.rgb + crackTex.rgb; // Overlay cracks
            o.Alpha = mainTex.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
