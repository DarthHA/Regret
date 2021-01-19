sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float uOpacity;
float3 uSecondaryColor;
float uTime;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uImageOffset;
float uIntensity;
float uProgress;
float2 uDirection;
float2 uZoom;
float2 uImageSize0;
float2 uImageSize1;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{ 
	float4 color = tex2D(uImage0, coords);
	float4 color1 = tex2D(uImage1, coords - float2(uColor.r, uColor.g));
	if (!any(color1))
		return color;

	float d = sqrt(color1.r * color1.r + color1.g * color1.g + color1.b * color1.b) * 100;
	float2 p = coords - float2(0.5, 0.5);
	p = p / sqrt(p.x * p.x + p.y * p.y);
	p.x *= 2 / uScreenResolution.x;
	p.y *= 2 / uScreenResolution.y;
	p = p * d;
	color = tex2D(uImage0, coords - p);
	d = (color.r + color.g + color.b) / 3;
	color.r = d + (color.r - d) * 0.75;
	color.g = d + (color.g - d) * 0.75;
	color.b = d + (color.b - d) * 0.75;
	return color;
}

technique Technique1
{
    pass BrokenEffect
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}