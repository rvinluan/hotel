Shader "Hidden/Custom Vignette Effect" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Fog ("Fog", Color) = (1,1,1,1)
	_Radius ("Radius", Float) = 0.1
	_Smoothness ("Smoothness", Float) = 0.5
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform fixed _Radius;
uniform fixed _Smoothness;
uniform fixed4 _Fog;

fixed4 frag (v2f_img i) : SV_Target
{
	fixed4 orig = tex2D(_MainTex, i.uv);
	fixed len = distance(fixed2(0.5, i.uv.y), fixed2(0.5, 0.5));
	fixed v = smoothstep(_Radius, _Smoothness, len);
	return lerp(orig, _Fog, v);
	// return fixed4(v,v,v, orig.a);
}
ENDCG

	}
}

Fallback off

}
