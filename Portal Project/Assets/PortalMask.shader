Shader "Portals/PortalMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
     {
        Blend One Zero
         Tags
         {
             "RenderType" = "Opaque"
             "RenderPipeline" = "UniversalPipeline"
         }
         
         HLSLINCLUDE
            #include  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
         ENDHLSL
         
        Pass
        {
            Name "Mask"
            
            
            HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;

            };

            struct v2f
            {
                float4 screenPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            uniform sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.screenPos = ComputeScreenPos(o.vertex);
                
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 uv = i.screenPos.xy/ i.screenPos.w;
                float4 col = tex2D(_MainTex, uv);
                
                return col;
            }
            ENDHLSL
        }
    }
}
