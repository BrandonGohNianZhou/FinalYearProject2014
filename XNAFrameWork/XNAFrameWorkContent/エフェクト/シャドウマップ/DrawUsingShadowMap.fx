float4x4 World;
float4x4 View;
float4x4 Projection;

texture ShadowMap;
sampler ShadowMapSampler = sampler_state
{
    Texture = (ShadowMap);
};

float4x4 LightView;
float4x4 LightProjection;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 PositionOnShadowMap : TEXCOORD0;
    float4 Color : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    
    output.Color = input.Color;
    
    output.PositionOnShadowMap = mul(
        worldPosition, 
        mul(LightView, LightProjection)
        );

    return output;
}

bool isLighted(float4 positionOnShadowMap)
{
    float2 texCoord;
    texCoord.x = (positionOnShadowMap.x / positionOnShadowMap.w + 1) / 2;
    texCoord.y = (-positionOnShadowMap.y / positionOnShadowMap.w + 1) / 2;
	
    //誤差があるはずなので、光が当たっているかどうかは
   //ほんの少しだけ甘く判定します。
   return positionOnShadowMap.z <= tex2D(ShadowMapSampler, texCoord).x + 0.001f;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    if(isLighted(input.PositionOnShadowMap))
        return input.Color;
    else 
        return input.Color / 3;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
