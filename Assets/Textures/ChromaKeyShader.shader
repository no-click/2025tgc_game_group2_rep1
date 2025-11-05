Shader "Custom/ChromaKeyShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {} // VideoPlayerが参照するテクスチャ
        _ChromaColor ("Chroma Key Color", Color) = (0,0,1,1) // 青
        _Threshold ("Threshold", Range(0,1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _ChromaColor;
            float _Threshold;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float diff = distance(col.rgb, _ChromaColor.rgb);
                if (diff < _Threshold)
                    col.a = 0;
                return col;
            }
            ENDCG
        }
    }
}
