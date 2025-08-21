Shader "Mirror/Window_URP"
{
    Properties
    {
        _Mask ("Mask", Int) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalRenderPipeline"
            "Queue"="Geometry-2"
        }

        Pass
        {
            Name "StencilPass"
            ColorMask 0
            ZWrite Off

            Stencil
            {
                Ref [_Mask]
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                return half4(0,0,0,0); // 実際は描かない（ColorMask 0なので無視される）
            }
            ENDHLSL
        }
    }
}
