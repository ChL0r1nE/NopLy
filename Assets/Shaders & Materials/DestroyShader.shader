Shader "Custom/DestroyShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Amount ("Amount", Range(0,1)) = 0
        _Mark ("Mark", Range(0, 1)) = 0
    }

    SubShader
    {
        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        fixed4 _Color;
        half _Amount;
        half _Mark;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            half value = tex2D(_MainTex, IN.uv_MainTex).r;
            clip(value - _Amount);

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            if(value  - _Amount < _Mark)
            o.Emission = fixed3(1,1,1);
        }

        ENDCG
    }

    FallBack "Diffuse"
}
