Shader "Custom/LCDScreen"
{
    Properties
    {
        [PerRendererData]_Diff ("Diffuse", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
		_PixTex("Pixel Texture", 2D) = "white"{}
		_Width("Grid Width", Int) = 1
		_Height("Grid Height", Int) = 1
		[HDR]_EmColor("Emission", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
		_SpeedX("X Speed", Float) = -1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


        struct Input
        {
            float2 uv_Diff;
			float2 uv_PixTex;
        };

        sampler2D _Diff;
        fixed4 _Color;
		sampler2D _PixTex;
		int _Width;
		int _Height;
		fixed4 _EmColor;
        half _Glossiness;
		half _OffsetX;
		half _SpeedX;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float uv_X_Diff;
			float uv_Y_Diff;

			/*uv_X_Diff = IN.uv_Diff.x;
			uv_Y_Diff = IN.uv_Diff.y;*/

			
			uv_X_Diff = _Width / 0.5;
			uv_X_Diff += IN.uv_Diff.x + _SpeedX * _Time.y;
			uv_X_Diff *= _Width;
			uv_X_Diff = floor(uv_X_Diff);
			uv_X_Diff /= _Width;
			uv_X_Diff += _Width / -0.5;

			uv_Y_Diff = _Height / 0.5;
			uv_Y_Diff += IN.uv_Diff.y;
			uv_Y_Diff *= _Height;
			uv_Y_Diff = floor(uv_Y_Diff);
			uv_Y_Diff /= _Height;
			uv_Y_Diff += _Height / -0.5;
			


			float2 diff_uv = float2(uv_X_Diff, uv_Y_Diff);
            
			float uv_X_PixTex;
			float uv_Y_PixTex;

			uv_X_PixTex = IN.uv_PixTex.x;
			uv_X_PixTex *= _Width;

			uv_Y_PixTex = IN.uv_PixTex.y;
			uv_Y_PixTex *= _Height;

			float2 pixTex_uv = float2(uv_X_PixTex, uv_Y_PixTex);

            o.Albedo = tex2D(_Diff, diff_uv).rgb * tex2D(_PixTex, pixTex_uv).rgb * _Color;
			o.Emission = tex2D(_Diff, diff_uv).rgb * tex2D(_PixTex, pixTex_uv).rgb * _EmColor;
            o.Smoothness = _Glossiness;
			o.Metallic = 0;
			o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
