Shader "Unity Answers/InvertColor" { 
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
       // [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            //"IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            //"PreviewType" = "Plane"
            //"CanUseSpriteAtlas" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#pragma multi_compile _ PIXELSNAP_ON

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord  : TEXCOORD0;
            };

            Interpolators vert(MeshData IN)
            {
                Interpolators OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                //#ifdef PIXELSNAP_ON
                //OUT.vertex = UnityPixelSnap(OUT.vertex);
                //#endif

                return OUT;
            }

            sampler2D _MainTex;

            
            float4 frag(Interpolators IN) : SV_Target
            {
                float4 color = tex2D(_MainTex, IN.texcoord);
                //color.rgb = 1- dstColor.rgb;
                return color;
            }
            
            ENDCG
        }
    }
}
