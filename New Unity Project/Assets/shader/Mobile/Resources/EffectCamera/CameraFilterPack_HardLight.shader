///////////////////////////////////////////
//  CameraFilterPack v2.0 - by VETASOFT 2015 ///
///////////////////////////////////////////

Shader "CameraFilterPack/HardLight" { 
Properties 
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_ScreenResolution ("_ScreenResolution", Vector) = (0.,0.,0.,0.)
}
SubShader
{
Pass
{
ZTest Always
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma target 3.0
#pragma glsl
#include "UnityCG.cginc"
uniform sampler2D _MainTex;
uniform float _Value;
uniform float _Value2;
uniform float4 _ScreenResolution;
uniform float2 _MainTex_TexelSize;
struct appdata_t
{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;
};
struct v2f
{
half2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
fixed4 color    : COLOR;
};
v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}

float hardLight( float s, float d )
{
	return (s < 0.5) ? 2.0 * s * d : 1.0 - 2.0 * (1.0 - s) * (1.0 - d);
}

float3 hardLight( float3 s, float3 d )
{
	float3 c;
	c.x = hardLight(s.x,d.x);
	c.y = hardLight(s.y,d.y);
	c.z = hardLight(s.z,d.z);
	return c;
}
float4 frag (v2f i) : COLOR
{
float2 uv=i.texcoord.xy;

float4 tex = tex2D(_MainTex,uv);    
#if UNITY_UV_STARTS_AT_TOP
if (_MainTex_TexelSize.y < 0)
uv.y = 1-uv.y;
#endif  
//float4 tex2 = tex2D(_MainTex2,uv);

float3 color =float3(1,1,1);

float3 S1=lerp(tex.rgb,color,_Value2);
float3 S2=lerp(tex.rgb,color,1-_Value2);

S1=lerp(S1,hardLight(S1,S2),_Value);
return  float4(S1,1.0);
}
ENDCG
}
}
}