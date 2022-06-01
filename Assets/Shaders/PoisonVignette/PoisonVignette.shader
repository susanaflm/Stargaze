Shader "Unlit/PoisonVignette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InnerRing ("Inner Ring", Range (0,1)) = 0.0
        _OuterRing ("Outer Ring", Range (0,1)) = 1.0
        _VignetteColor("Vignette Color", Color) = (0,1,0,1)
        _VignettePosition ("Center Position", Vector) = (0.5,0.5,1,1)
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
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float _InnerRing;
            float _OuterRing;
            float _VignetteColor;
            half4 _VignettePosition;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _VignetteColor;
                half2 center = half2(_VignettePosition.x, _VignettePosition.y);
                float dist = distance(center, i.uv) * 1.414213; //Math trick that gets the distance to fit in [0,1]
                float vig = clamp((_OuterRing - dist) / (_OuterRing - _InnerRing), 0.0, 1.0);
                return col * vig;
            }
            ENDCG
        }
    }
}
