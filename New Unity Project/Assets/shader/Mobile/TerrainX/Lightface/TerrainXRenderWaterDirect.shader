// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "GOE/Scene/TerrainXRenderWaterDirect" 
{
Properties 
{
	_Color ("Main Color11", Color) = (1,1,1,1)
	_Splat0 ("Layer0 (R)", 2D) = "white" {}
	_Splat1 ("Layer1 (G)", 2D) = "white" {}
	_Splat2 ("Layer2 (G)", 2D) = "white" {}
	_Splat3("Layer3 (G)", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "red" {}

	_IllumFactor ("Illumin Factor", Range(1,2)) = 1
	_WaterMaskTex("Water Mask Tex(RGB)", 2D) = "black" {}
	_Layer2BumpTex("Layer2 Bump Tex", 2D) = "bump"{}
	_Layer2BumpIntensity("Layer2 BumpIntensity", Range(0,1)) = 0
	_CubeMap("CubeMap", Cube) = "_Skybox" {}
	_CubeMapIntensity("CubeMapIntensity", Range(0, 4)) = 0

	_Gloss("Gloss", Range(1,11)) = 8
	_MySpecColor("Spec Color", Color) = (0.5,0.5,0.5,0.5)
	_MyDiffColor("Diff Color", Color) = (0.5,0.5,0.5,0.5)

	_Lux_RainRippleWater("Rain: Ripple(RG) Water(BA)", vector) = (0.6, 1, 1, 1)
	_Lux_SnowAlbedoNormal("Snow: Albedo(RG) Normal(BA)", vector) = (1,1,1,1)
}
	
SubShader 
{
	Tags{ "Queue" = "Geometry" }
	Pass 
	{
		Tags{ "LightMode" = "ForwardBase" }
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma multi_compile_fwdbase
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
			#pragma multi_compile SM_3_OFF SM_3_ON
			#pragma multi_compile LIGHT_FACE_OFF LIGHT_FACE_ON 
			#pragma multi_compile NORMAL_ON NORMAL_OFF
			#pragma multi_compile  RAIN_ON RAIN_OFF
			#pragma multi_compile SNOW_OFF SNOW_ON
			#pragma exclude_renderers d3d11
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "LightFace.cginc"
			#include "Weather.cginc"
			
			sampler2D _Control;
			sampler2D _Splat0;
			sampler2D _Splat1;
			sampler2D _Splat3;
			sampler2D _Splat2;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			float4 _Control_ST;
			
			float4 _Color;
			float _IllumFactor;

			sampler2D _WaterMaskTex;
			float4 _WaterMaskTex_ST;
			sampler2D _Layer2BumpTex;
			float4 _Layer2BumpTex_ST;
			float _Layer2BumpIntensity;
			samplerCUBE _CubeMap;
			float _CubeMapIntensity;

			float _Gloss;
			fixed4 _MySpecColor;
			fixed4 _MyDiffColor;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				LIGHTMAP_COORDS(1)
				float4 tangent : TANGENT;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4	pos : SV_POSITION;
				float4	uv : TEXCOORD0;
				LIGHTFACE_COORDS(1)
				FOG_COORDS(2)
				NORMAL_COORDS(3, 4, 5, 6)
			};
			

			v2f vert (appdata v)
			{
				WORLD_POS
				v2f o;
				o.uv.xy = v.texcoord.xy;
				o.pos = UnityObjectToClipPos(float4(v.vertex.xyz, 1));

				LIGHTMAP_VS
				LIGHTFACE_VS
				LIGHTFACE_VS_SHADOW
				FOG_VS
				NORMAL_VS

				return o;
			}
				

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 splat_control = tex2D(_Control, i.uv.xy);
				float2 splat0_uv = TRANSFORM_TEX(i.uv.xy, _Splat0);
				float2 splat1_uv = TRANSFORM_TEX(i.uv.xy, _Splat1);
				float2 splat2_uv = TRANSFORM_TEX(i.uv.xy, _Splat2);
				float2 splat3_uv = TRANSFORM_TEX(i.uv.xy, _Splat3);
#if ((defined (SM_3_ON)) && (defined (RAIN_ON)))
				int showRipple = step(0.7, i.normalDir.y);
				fixed3 rippleNormal = AddWaterRipples(1, i.posWorld, 1, 1);
				splat0_uv += showRipple * _Lux_RainRippleWater.y * rippleNormal.xy;
				splat1_uv += showRipple * _Lux_RainRippleWater.y  * rippleNormal.xy;
				splat2_uv += showRipple * _Lux_RainRippleWater.y  * rippleNormal.xy;
				splat3_uv += showRipple * _Lux_RainRippleWater.y  * rippleNormal.xy;
#endif
				float4 splat0Tex = tex2D(_Splat0, splat0_uv.xy);
				float4 splat1Tex = tex2D(_Splat1, splat1_uv.xy);
				float4 splat2Tex = tex2D(_Splat2, splat2_uv.xy);
				float4 splat3Tex = tex2D(_Splat3, splat3_uv.xy);

				fixed4 tex = 1;
				tex.rgb  = splat_control.r * splat0Tex.rgb;
				tex.rgb += splat_control.g * splat1Tex.rgb;
				tex.rgb += splat_control.b * splat2Tex.rgb;
				tex.rgb += splat_control.a * splat3Tex.rgb;
				tex.rgb *= _Color.rgb * _IllumFactor;

				float3 nomalizedNormal = normalize(i.normalDir);
#if ((defined (SM_3_ON)) && (defined (NORMAL_ON)))
				float3 normalDirection = NORMAL_PS_Normal(_Layer2BumpTex, TRANSFORM_TEX(i.uv.xy, _Layer2BumpTex), nomalizedNormal, i.tangentDir, i.bitangentDir, _Layer2BumpIntensity);
#else
				float3 normalDirection = nomalizedNormal;
#endif

				float waterMask = tex2D(_WaterMaskTex, TRANSFORM_TEX(i.uv.xy, _WaterMaskTex)).r;
				normalDirection = lerp(normalDirection, nomalizedNormal, saturate(waterMask + 0.3));

#if ((defined (SM_3_ON)) && (defined (RAIN_ON)))
				normalDirection = NORMAL_PS_Local2World(rippleNormal, normalDirection, i.tangentDir, i.bitangentDir, 1);
#endif

				float3 viewDirection = normalize(UnityWorldSpaceViewDir(i.posWorld.xyz));
				float3 lightDirection = normalize(UnityWorldSpaceLightDir(i.posWorld.xyz));
				float3 halfDirection = normalize(viewDirection + lightDirection);
				float3 reflectDirection = reflect(-viewDirection, normalDirection);
				float spec = saturate(dot(normalDirection, halfDirection));
				float diff = saturate(dot(normalDirection, lightDirection));
				//diff = saturate(pow(diff, _DiffGloss));

				float3 diffColor = /*tex.rgb * */diff * _MyDiffColor* splat_control.b;
				float3 specColorLayer2 = pow(spec, exp2(_Gloss)) * _MySpecColor.rgb * 2 * splat2Tex.a * splat_control.b/*法线区域才有高光*/;
				//water
				tex.rgb = lerp(tex.rgb, texCUBE(_CubeMap, reflectDirection).rgb * _CubeMapIntensity, waterMask);
				tex.rgb = lerp(tex.rgb, 0.7 * tex.rgb, waterMask);
				tex.rgb = tex.rgb + diffColor + specColorLayer2;

				LIGHTMAP_PS
				LIGHTFACE_PS_SHADOW
				SNOW_PS_BUMP
				FOG_PS
					
				return tex;

			}

		ENDCG
	}	
}

FallBack "Diffuse"
}

