Shader "Mirror/Window"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-2" }

        Pass
        {
            ColorMask 0
            ZWrite Off

            Stencil
            {
                Ref 2
                Comp Always
                Pass Replace
            }
        }
    }
}
