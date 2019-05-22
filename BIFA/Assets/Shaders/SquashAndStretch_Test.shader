// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BIFA/SquashAndStretch"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Squash("Squash", Float) = 0
		_Radius("Radius", Float) = 1
		_SquashEffect("SquashEffect", Float) = 1
		_SquashCurve("SquashCurve", Float) = 0
		_StretchEffect("StretchEffect", Float) = 1
		_StretchCurve("StretchCurve", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			half2 uv_texcoord;
		};

		uniform half _Radius;
		uniform half _StretchCurve;
		uniform half _StretchEffect;
		uniform half _Squash;
		uniform half _SquashEffect;
		uniform half _SquashCurve;
		uniform sampler2D _NormalMap;
		uniform sampler2D _Albedo;
		uniform half _Metallic;
		uniform half _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 break49_g1 = ase_vertex3Pos;
			float xPos52_g1 = break49_g1.x;
			float yPos50_g1 = break49_g1.y;
			float Radius329 = _Radius;
			float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
			float temp_output_22_0_g1 = ( 1.0 - ( ( sin( ( ( ( abs( yPos50_g1 ) / ( Radius329 / ase_objectScale.x ) ) * ( 0.5 * UNITY_PI ) ) - ( 0.5 * UNITY_PI ) ) ) + 1.0 ) / 2.0 ) );
			float StretchCurve333 = _StretchCurve;
			float StretchEffect332 = _StretchEffect;
			float Squash328 = _Squash;
			float SquashInput77_g1 = Squash328;
			float clampResult18_g1 = clamp( SquashInput77_g1 , -10.0 , 1.0 );
			float Squas23_g1 = clampResult18_g1;
			float lerpResult41_g1 = lerp( 1.0 , ( ( 1.0 - ( temp_output_22_0_g1 * StretchCurve333 ) ) * StretchEffect332 ) , ( atan( ( abs( Squas23_g1 ) * 2.0 ) ) / ( 0.5 * UNITY_PI ) ));
			float StretchMultiplierXZZ45_g1 = lerpResult41_g1;
			float SquashEffect330 = _SquashEffect;
			float SquashCurve331 = _SquashCurve;
			float lerpResult38_g1 = lerp( 0.0 , ( SquashEffect330 * ( 1.0 - ( SquashCurve331 * temp_output_22_0_g1 ) ) ) , Squas23_g1);
			float SquashMultiplierXZZ43_g1 = ( lerpResult38_g1 + 1.0 );
			float clampResult66_g1 = clamp( ( ( Squas23_g1 * 1000.0 ) + 0.5 ) , 0.0 , 1.0 );
			float lerpResult69_g1 = lerp( StretchMultiplierXZZ45_g1 , SquashMultiplierXZZ43_g1 , clampResult66_g1);
			float SquashMultiplierYY47_g1 = ( ( 1.0 - Squas23_g1 ) * yPos50_g1 );
			float zPos51_g1 = break49_g1.z;
			float3 appendResult75_g1 = (half3(( xPos52_g1 * lerpResult69_g1 ) , SquashMultiplierYY47_g1 , ( zPos51_g1 * lerpResult69_g1 )));
			v.vertex.xyz = appendResult75_g1;
			half4 tex2DNode338 = tex2Dlod( _NormalMap, half4( v.texcoord.xy, 0, 0.0) );
			v.normal = tex2DNode338.rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			half4 tex2DNode338 = tex2D( _NormalMap, i.uv_texcoord );
			o.Normal = tex2DNode338.rgb;
			o.Albedo = tex2D( _Albedo, i.uv_texcoord ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15900
7;7;1906;1014;1927.385;-1030.849;1.02133;True;True
Node;AmplifyShaderEditor.RangedFloatNode;5;-1626.63,1779.717;Float;False;Property;_Squash;Squash;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-1623.435,2146.656;Float;False;Property;_StretchEffect;StretchEffect;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1627.613,1955.967;Float;False;Property;_SquashEffect;SquashEffect;6;0;Create;True;0;0;False;0;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1624.789,2054.804;Float;False;Property;_SquashCurve;SquashCurve;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-1622.294,2236.43;Float;False;Property;_StretchCurve;StretchCurve;9;0;Create;True;0;0;False;0;0;1.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1622.342,1868.239;Float;False;Property;_Radius;Radius;5;0;Create;True;0;0;False;0;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;331;-1318.504,2054.479;Float;False;SquashCurve;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;333;-1318.906,2236.88;Float;False;StretchCurve;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;343;-1386.08,1307.63;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;330;-1319.702,1956.279;Float;False;SquashEffect;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;326;-1305.03,1619.703;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;328;-1323.702,1780.28;Float;False;Squash;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;332;-1319.506,2146.68;Float;False;StretchEffect;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;329;-1320.904,1868.881;Float;False;Radius;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;341;-1553.024,1604.549;Float;False;Object;World;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;336;-886.8337,1760.4;Float;False;SquashAndStretch;-1;;1;f8d354f41350d51489ee32fd0240afc5;0;7;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;338;-916.4666,1283.913;Float;True;Property;_NormalMap;Normal Map;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;337;-925.6921,1082.281;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;3194da065221fd848adb46ba98096ca2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;342;-901.3154,1478.952;Float;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;339;-905.335,1559.876;Float;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;0;0.58;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-453.6772,1358.696;Half;False;True;2;Half;ASEMaterialInspector;0;0;Standard;BIFA/SquashAndStretch;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Absolute;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;331;0;42;0
WireConnection;333;0;87;0
WireConnection;330;0;61;0
WireConnection;328;0;5;0
WireConnection;332;0;154;0
WireConnection;329;0;14;0
WireConnection;336;1;326;0
WireConnection;336;2;328;0
WireConnection;336;4;329;0
WireConnection;336;5;330;0
WireConnection;336;3;331;0
WireConnection;336;6;332;0
WireConnection;336;7;333;0
WireConnection;338;1;343;0
WireConnection;337;1;343;0
WireConnection;0;0;337;0
WireConnection;0;1;338;0
WireConnection;0;3;342;0
WireConnection;0;4;339;0
WireConnection;0;11;336;0
WireConnection;0;12;338;0
ASEEND*/
//CHKSM=773CFD42AED5172666EE02776BE3ECA796A5F04C