#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0
    #define PS_SHADERMODEL ps_4_0
#endif

float4x4 WorldViewProjection;
float4x4 LightWorldViewProjection;
float4x4 World;
float3 LightPos;
float LightPower;
float Ambient;

float4 AmbientColor;

Texture2D LightCookie;
sampler LightCookieSampler = sampler_state
{
    texture = <LightCookie>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = mirror;
    AddressV = mirror;
};

Texture2D Texture1;
sampler TextureSampler1 = sampler_state
{
    texture = <Texture1>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = mirror;
    AddressV = mirror;
};

Texture2D ShadowMap;
sampler ShadowMapSampler = sampler_state
{
    texture = <ShadowMap>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = clamp;
    AddressV = clamp;
};

//POINT LIGHT

struct VertexToPixel
{
    float4 Position     : SV_Position;
    float2 TexCoords    : TEXCOORD0;
    float4 Color        : COLOR0;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);
}

VertexToPixel BasicVertexShader(float4 Position : POSITION)
{
    VertexToPixel Out;
    Out.Position = mul(Position, WorldViewProjection);
    return Out;
}

PixelToFrame BasicPixelShader(VertexToPixel input) : COLOR
{
    PixelToFrame Out;
	return Out;
}

technique BasicColorDrawing
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL BasicVertexShader();
        PixelShader = compile PS_SHADERMODEL BasicPixelShader();
    }
}

//SHADOW MAP

struct CreateShadowMap_VSOut
{
    float4 Position : POSITION;
    float2 Depth : TEXCOORD0;
};

//  CREATE SHADOW MAP
CreateShadowMap_VSOut CreateShadowMap_VertexShader(float4 Position : POSITION)
{
    CreateShadowMap_VSOut Out;
    Out.Position = mul(Position, LightWorldViewProjection);
    Out.Depth = Out.Position.zw;
    return Out;
}

float4 CreateShadowMap_PixelShader(CreateShadowMap_VSOut input) : COLOR
{
    return float4(input.Depth.x / input.Depth.y, 0, 0, 1);
}


technique ShadowMap
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL CreateShadowMap_VertexShader();
        PixelShader = compile PS_SHADERMODEL CreateShadowMap_PixelShader();
    }
}

//SHADOWED SCENE

struct SSceneVertexToPixel
{
    float4 Position : SV_Position;
    float4 Pos2DAsSeenByLight : TEXCOORD0;

    float2 TexCoords : COLOR0;
    float3 Normal : COLOR0;
    float4 Position3D : TEXCOORD0;
};

struct SScenePixelToFrame
{
    float4 Color : COLOR0;
};


SSceneVertexToPixel ShadowedSceneVertexShader(float4 inPos : SV_Position, float2 inTexCoords : TEXCOORD0, float3 inNormal : NORMAL)
{
    SSceneVertexToPixel Output = (SSceneVertexToPixel)0;

    Output.Position = mul(inPos, WorldViewProjection);
    Output.Pos2DAsSeenByLight = mul(inPos, LightWorldViewProjection);
    Output.Normal = normalize(mul(inNormal, (float3x3)World));
    Output.Position3D = mul(inPos, World);
    Output.TexCoords = inTexCoords;

    return Output;
}

SScenePixelToFrame ShadowedScenePixelShader(SSceneVertexToPixel PSIn)
{
    SScenePixelToFrame Output = (SScenePixelToFrame)0;

    float2 ProjectedTexCoords;
    ProjectedTexCoords[0] = PSIn.Pos2DAsSeenByLight.x / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;
    ProjectedTexCoords[1] = -PSIn.Pos2DAsSeenByLight.y / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;

    float diffuseLightingFactor = 0;
    float intensityByCookie = 1;
    if ((saturate(ProjectedTexCoords).x == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords).y == ProjectedTexCoords.y))
    {
        float depthStoredInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords).r;
        float realDistance = PSIn.Pos2DAsSeenByLight.z / PSIn.Pos2DAsSeenByLight.w;
        if ((realDistance - 1.0f / 100.0f) <= depthStoredInShadowMap)
        {
            diffuseLightingFactor = DotProduct(LightPos, PSIn.Position3D, PSIn.Normal);
            diffuseLightingFactor = saturate(diffuseLightingFactor);
            intensityByCookie = saturate(tex2D(LightCookieSampler, ProjectedTexCoords).r);
            diffuseLightingFactor *= LightPower;
        }
    }

    float4 baseColor = tex2D(TextureSampler1, PSIn.TexCoords);
    Output.Color.rgb = baseColor.rgb * saturate(Ambient + (intensityByCookie * diffuseLightingFactor));
    Output.Color.a = baseColor.a;

    return Output;
}


technique ShadowedScene
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL ShadowedSceneVertexShader();
        PixelShader = compile PS_SHADERMODEL ShadowedScenePixelShader();
    }
}