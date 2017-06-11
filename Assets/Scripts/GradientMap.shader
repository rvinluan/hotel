Shader "Hidden/Gradient Map Effect" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_RampTex ("Base (RGB)", 2D) = "grayscaleRamp" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _RampTex;

fixed4 frag (v2f_img i) : SV_Target
{
	fixed4 orig = tex2D(_MainTex, i.uv);
	
	//calculate brightness
	fixed lum = (0.212 * orig.rr) + (0.7152 * orig.gg) + (0.0722 * orig.bb);
	
	fixed4 color = tex2D(_RampTex, float2(lum, 0));
	//fixed4 color = fixed4(lum, lum, lum, orig.a);

	return color;
}
ENDCG

	}
}

Fallback off

}
