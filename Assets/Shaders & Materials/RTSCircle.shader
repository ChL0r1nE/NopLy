Shader "Custom/RTSCircle"
{
    Properties
    {
        _CircleColor ("CircleColor", Color) = (1,1,1,1)
        _CircleInsideColor ("CircleInsideColor", Color) = (1,1,1,1)
        _CirclePosition ("CirclePosition", Vector) = (0,0,0.0)
        _Angle ("Angle", Float ) = 2
        _Length ("Radius", Float ) = 2
        _Thickness ("Thickness", Float ) = 2
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            uniform float4 _CirclePosition;
            uniform fixed4 _CircleInsideColor;
            uniform fixed4 _CircleColor;
            uniform float _Angle;
            uniform float _Length;
            uniform float _Thickness;

            struct VertexInput 
            {
                float4 vertex : POSITION;
            };

            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            
            VertexOutput vert (VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(VertexOutput i) : COLOR 
            {
                float dist = distance(i.posWorld,_CirclePosition);

                float arctg = degrees(abs(atan((i.posWorld - _CirclePosition).x / (i.posWorld - _CirclePosition).z)));
                if(arctg > 45) arctg = arctg - 45;

                float artg = degrees(abs(atan((i.posWorld - _CirclePosition).z / (i.posWorld - _CirclePosition).x)));
                if(artg > 45) artg = artg - 45;

                clip(arctg < _Angle ? -1 : artg < _Angle ? -1 : 1);
                clip(step((dist-_Thickness),_Length)-step(dist,_Length) == 0 ? -1 : 1);

                return _CircleColor;
            }

            ENDCG
        }
    }
}