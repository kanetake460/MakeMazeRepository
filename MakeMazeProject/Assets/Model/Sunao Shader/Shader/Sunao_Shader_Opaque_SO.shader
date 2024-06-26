﻿//--------------------------------------------------------------
//              Sunao Shader Stencil    Ver 1.2.2
//
//                      Copyright (c) 2020 揚茄子研究所
//                              Twitter : @SUNAO_VRC
//                              VRChat  : SUNAO_
//
// This software is released under the MIT License.
// see LICENSE or http://sunao.orz.hm/agenasulab/ss/LICENSE
//--------------------------------------------------------------

Shader "Sunao Shader/[Stencil Outline]/Opaque" {


	Properties {

		_MainTex           ("Main Texture"              , 2D) = "white" {}
		_Color             ("Color"                     , Color) = (1,1,1,1)
		_Alpha             ("Alpha"                     , Range( 0.0,  2.0)) = 1.0
		_Cutout            ("Cutout"                    , Range( 0.0,  1.0)) = 0.5

		[NoScaleOffset][Normal]
		_BumpMap           ("Normal Map"                , 2D) = "bump" {}
		[NoScaleOffset]
		_OcclusionMap      ("Occlusion"                 , 2D) = "white" {}
		[NoScaleOffset]
		_AlphaMask         ("Alpha Mask"                , 2D) = "white" {}

		_Bright            ("Brightness"                , Range( 0.0,  1.0)) = 1.0
		_BumpScale         ("Normal Map Scale"          , Range(-2.0,  2.0)) = 1.0
		_OcclusionStrength ("Occlusion Strength"        , Range( 0.0,  1.0)) = 1.0
		[Enum(SH Light, 0 , Main Texture , 1 , Final Color , 2)]
		_OcclusionMode     ("Occlusion Mode    "        , int) = 0
		_AlphaMaskStrength ("Alpha Mask Strength"       , Range( 0.0,  1.0)) = 1.0
		[SToggle]
		_VertexColor       ("Use Vertex Color"          , int) = 0


		[NoScaleOffset]
		_ShadeMask         ("Shade Mask"                , 2D) = "white" {}
		_Shade             ("Shade Strength"            , Range( 0.0,  1.0)) = 0.3
		_ShadeWidth        ("Shade Width"               , Range( 0.0,  2.0)) = 0.75
		_ShadeGradient     ("Shade Gradient"            , Range( 0.0,  2.0)) = 0.75
		_ShadeColor        ("Shade Color"               , Range( 0.0,  1.0)) = 0.5
		_CustomShadeColor  ("Custom Shade Color"        , Color) = (0,0,0,0)

		[SToggle]
		_ToonEnable        ("Enable Toon Shading"       , int) = 0
		[IntRange]
		_Toon              ("Toon"                      , Range( 0.0,  9.0)) = 9.0
		_ToonSharpness     ("Toon Sharpness"            , Range( 0.0,  1.0)) = 1.0

		[NoScaleOffset]
		_LightMask         ("Lighting Boost Mask"       , 2D) = "black" {}
		_LightBoost        ("Lighting Boost"            , Range( 1.0,  5.0)) = 3.0
		_Unlit             ("Unlighting"                , Range( 0.0,  1.0)) = 0.0
		[SToggle]
		_MonochromeLit     ("Monochrome Lighting"       , int) = 0


		[SToggle]
		_OutLineEnable     ("Enable Outline"            , int) = 0
		[NoScaleOffset]
		_OutLineMask       ("Outline Mask"              , 2D) = "white" {}
		_OutLineColor      ("Outline Color"             , Color) = (0,0,0,1)
		_OutLineSize       ("Outline Scale"             , Range( 0.0,  1.0)) = 0.1
		[SToggle]
		_OutLineLighthing  ("Use Light Color"           , int) = 1
		[SToggle]
		_OutLineTexColor   ("Use Main Texture"          , int) = 0
		[NoScaleOffset]
		_OutLineTexture    ("Outline Texture"           , 2D) = "white" {}
		[SToggle]
		_OutLineFixScale   ("x10 Scale"                 , int) = 0


		[SToggle]
		_EmissionEnable    ("Enable Emission"           , int) = 0
		_EmissionMap       ("Emission Mask"             , 2D) = "white" {}
		[HDR]
		_EmissionColor     ("Emission Color"            , Color) = (1,1,1)
		_Emission          ("Emission Intensity"        , Range( 0.0,  2.0)) = 1.0

		_EmissionMap2      ("2nd Emission Mask"         , 2D) = "white" {}
		[Enum(Add , 0 ,Multiply , 1 , Minus , 2)]
		_EmissionMode      ("Emission Mode"             , int) = 0
		_EmissionBlink     ("Blink"                     , Range( 0.0,  1.0)) = 0.0
		_EmissionFrequency ("Frequency"                 , Range( 0.0,  5.0)) = 1.0
		[Enum(Sine , 0 , Saw , 1 , SawR , 2 , Square , 3)]
		_EmissionWaveform  ("Waveform"                  , int) = 0
		_EmissionScrX      ("Scroll X"                  , Range(-10.0, 10.0)) = 0.0
		_EmissionScrY      ("Scroll Y"                  , Range(-10.0, 10.0)) = 0.0
		[SToggle]
		_EmissionLighting  ("Use Lighting"              , int) = 0
		[SToggle]
		_IgnoreTexAlphaE   ("Ignore Texture Alpha"      , int) = 0
		_EmissionInTheDark ("Only in the Dark"          , Range(  0.0,  1.0)) = 0.0


		[SToggle]
		_ParallaxEnable    ("Enable Parallax Emission"  , int) = 0
		_ParallaxMap       ("Parallax Emission Mask"    , 2D) = "white" {}
		[HDR]
		_ParallaxColor     ("Emission Color"            , Color) = (1,1,1)
		_ParallaxEmission  ("Emission Intensity"        , Range( 0.0,  2.0)) = 1.0
		_ParallaxDepth     ("Parallax Depth"            , Range( 0.0,  1.0)) = 1.0

		_ParallaxDepthMap  ("Parallax Depth Mask"       , 2D) = "black" {}
		_ParallaxMap2      ("2nd Parallax Emission Mask", 2D) = "white" {}
		[Enum(Add , 0 ,Multiply , 1 , Minus , 2)]
		_ParallaxMode      ("Emission Mode"             , int) = 0
		_ParallaxBlink     ("Blink"                     , Range( 0.0,  1.0)) = 0.0
		_ParallaxFrequency ("Frequency"                 , Range( 0.0,  5.0)) = 1.0
		[Enum(Sine , 0 , Saw , 1 , SawR , 2 , Square , 3)]
		_ParallaxWaveform  ("Waveform"                  , int) = 0
		_ParallaxPhaseOfs  ("Phase Offset"              , Range( 0.0,   1.0)) = 0.0
		_ParallaxScrX      ("Scroll X"                  , Range(-10.0, 10.0)) = 0.0
		_ParallaxScrY      ("Scroll Y"                  , Range(-10.0, 10.0)) = 0.0
		[SToggle]
		_ParallaxLighting  ("Use Lighting"              , int) = 0
		[SToggle]
		_IgnoreTexAlphaPE  ("Ignore Texture Alpha"      , int) = 0
		_ParallaxInTheDark ("Only in the Dark"          , Range(  0.0,  1.0)) = 0.0


		[SToggle]
		_ReflectionEnable  ("Enable Reflection"         , int) = 0
		[NoScaleOffset]
		_MetallicGlossMap  ("Reflection Mask"           , 2D) = "white" {}
		[NoScaleOffset]
		_MatCap            ("Material Capture"          , 2D) = "black" {}
		_Specular          ("Specular Intensity"        , Range( 0.0,  2.0)) = 1.0
		_Metallic          ("Metallic"                  , Range( 0.0,  1.0)) = 0.5
		_MatCapStrength    ("MatCap Strength"           , Range( 0.0,  2.0)) = 1.0
		_GlossMapScale     ("Smoothness"                , Range( 0.0,  1.0)) = 0.75
		[SToggle]
		_SpecularTexColor  ("Tex Color for Specular"    , int) = 0
		[SToggle]
		_MetallicTexColor  ("Tex Color for Metallic"    , int) = 1
		[SToggle]
		_MatCapTexColor    ("Tex Color for MatCap"      , int) = 0
		[SToggle]
		_SpecularSH        ("SH Light Specular"         , int) = 1
		[Enum(None , 0 , RealTime , 1 , SH , 2 , Both , 3)]
		_ReflectLit        ("Light Color for Reflection", int) = 0
		[Enum(None , 0 , RealTime , 1 , SH , 2 , Both , 3)]
		_MatCapLit         ("Light Color for MatCap"    , int) = 3
		[SToggle]
		_IgnoreTexAlphaR   ("Ignore Texture Alpha"      , int) = 0


		[SToggle]
		_RimLitEnable      ("Enable Rim Light"          , int) = 0
		[NoScaleOffset]
		_RimLitMask        ("Rim Light Mask"            , 2D) = "white" {}
		[HDR]
		_RimLitColor       ("Rim Light Color"           , Color) = (1,1,1,1)
		_RimLit            ("Rim Lighting"              , Range( 0.0,  2.0)) = 1.0
		_RimLitGradient    ("Rim Light Gradient"        , Range( 0.0,  2.0)) = 1.0
		[SToggle]
		_RimLitLighthing   ("Use Light Color"           , int) = 1
		[SToggle]
		_RimLitTexColor    ("Use Main Texture"          , int) = 0
		[Enum(Add , 0 , Multiply , 1 , Minus , 2)]
		_RimLitMode        ("Rim Light Mode"            , int) = 0
		[SToggle]
		_IgnoreTexAlphaRL  ("Ignore Texture Alpha"      , int) = 0


		[Enum(Off , 0 , Back , 2 , Front , 1)]
		_Culling           ("Culling"                   , int) = 0

		[SToggle]
		_EnableZWrite      ("Enabel Z Write"            , int) = 1

		_DirectionalLight  ("Directional Light"         , Range( 0.0,  2.0)) = 1.0
		_SHLight           ("SH Light"                  , Range( 0.0,  2.0)) = 1.0
		_PointLight        ("Point Light"               , Range( 0.0,  2.0)) = 1.0
		[SToggle]
		_LightLimitter     ("Light Limitter"            , int) = 1

		[SToggle]
		_EnableGammaFix    ("Enable Gamma Fix"          , int) = 0
		_GammaR            ("R Gamma"                   , Range( 0.0,  5.0)) = 1.0
		_GammaG            ("G Gamma"                   , Range( 0.0,  5.0)) = 1.0
		_GammaB            ("B Gamma"                   , Range( 0.0,  5.0)) = 1.0

		[SToggle]
		_EnableBlightFix   ("Enable Brightness Fix"     , int) = 0
		_BlightOutput      ("Output Brightness"         , Range( 0.0,  5.0)) = 1.0
		_BlightOffset      ("Brightness Offset"         , Range(-5.0,  5.0)) = 0.0

		[SToggle]
		_LimitterEnable    ("Enable Limitter"           , int) = 0
		_LimitterMax       ("Limitter Max"              , Range( 0.0,  5.0)) = 1.0


		[HideInInspector] _MainFO          ("MainFO"            , int) = 0
		[HideInInspector] _ShadingFO       ("Shading FO"        , int) = 0
		[HideInInspector] _OutlineFO       ("Outline FO"        , int) = 0
		[HideInInspector] _EmissionFO      ("Emission FO"       , int) = 0
		[HideInInspector] _ParallaxFO      ("Parallax FO"       , int) = 0
		[HideInInspector] _ReflectionFO    ("Reflection FO"     , int) = 0
		[HideInInspector] _RimLightingFO   ("Rim Lighting FO"   , int) = 0
		[HideInInspector] _OtherSettingsFO ("Other Settings FO" , int) = 0

		[HideInInspector] _VersionH        ("Version H"         , int) = 0
		[HideInInspector] _VersionM        ("Version M"         , int) = 0
		[HideInInspector] _VersionL        ("Version L"         , int) = 0

	}



	SubShader {

		LOD 0

		Tags {
			"IgnoreProjector" = "True"
			"RenderType"      = "Opaque"
			"Queue"           = "Geometry"
		}


		Pass {
			Tags {
				"LightMode"  = "ForwardBase"
			}

			Cull [_Culling]
			ZWrite [_EnableZWrite]

			Stencil {
				Ref 2
				Comp Always
				Pass Replace
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma target 4.5

			#define PASS_FB

			#include "./cginc/SunaoShader_Core.cginc"

			ENDCG
		}



		Pass {
			Tags {
				"LightMode"  = "ForwardBase"
			}

			Cull Front
			ZWrite [_EnableZWrite]

			Stencil {
				Ref 2
				Comp NotEqual
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma target 4.5

			#define PASS_OL_FB

			#include "./cginc/SunaoShader_OL.cginc"

			ENDCG
		}


		Pass {
			Tags {
				"LightMode"  = "ForwardAdd"
			}

			Cull Front
			Blend One One
			ZWrite Off

			Stencil {
				Ref 2
				Comp NotEqual
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdadd
			#pragma multi_compile_fog
			#pragma target 4.5

			#define PASS_OL_FA

			#include "./cginc/SunaoShader_OL.cginc"

			ENDCG
		}


		Pass {
			Tags {
				"LightMode"  = "ForwardAdd"
			}

			Cull [_Culling]
			Blend One One
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdadd
			#pragma multi_compile_fog
			#pragma target 4.5

			#define PASS_FA

			#include "./cginc/SunaoShader_Core.cginc"

			ENDCG
		}



		Pass {
			Tags {
				"LightMode" = "ShadowCaster"
			}

			ZWrite On
			ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma target 4.5

			#define PASS_SC

			#include "./cginc/SunaoShader_SC.cginc"

			ENDCG
		}
	}

	FallBack "Diffuse"

	CustomEditor "SunaoShader.GUI"
}
