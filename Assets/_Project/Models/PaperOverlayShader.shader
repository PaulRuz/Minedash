Shader "Custom/PaperOverlayMultiply"
{
    Properties
    {
        _OverlayTex ("Overlay Texture", 2D) = "white" {}
        _Intensity ("Overlay Intensity", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
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
                float4 pos : SV_POSITION;
            };

            sampler2D _OverlayTex;
            float _Intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i,sampler2D _MainTex) : SV_Target
            {
                // Получаем основной цвет сцены
                float4 sceneColor = tex2D(_MainTex, i.uv);
                
                // Получаем цвет текстуры бумаги
                float4 overlayColor = tex2D(_OverlayTex, i.uv);
                
                // Реализация эффекта Multiply с интерполяцией
                float3 multipliedColor = sceneColor.rgb * overlayColor.rgb;
                
                // Интерполяция между исходным цветом и цветом умножения
                float3 resultColor = lerp(sceneColor.rgb, multipliedColor, _Intensity);
                
                return float4(resultColor, sceneColor.a);
            }
            ENDCG
        }
    }
}