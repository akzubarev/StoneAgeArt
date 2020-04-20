Shader "Custom/Compare"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _MaskTex("Texture", 2D) = "white" {}
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            BlendOp Add
            Blend One Zero
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
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                sampler2D _MaskTex;
                float4 _MaskTex_ST;
  
                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 colTex = tex2D(_MainTex, i.uv);
                    fixed d = length(colTex);
                    fixed4 colMask = tex2D(_MaskTex, i.uv);
                    if (colMask.w > 0.5) {
                        if (d > 0.1)
                            return fixed4(0, 1, 0, 1);
                        else
                            return fixed4(0, 0, 1, 1);
                    }
                    else {
                        if (d > 0.1)
                            return fixed4(1, 0, 0, 1);
                    }
                    
                    return fixed4(0, 0, 0, 0);
                }
                ENDCG
            }
        }
}
