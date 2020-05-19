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

Texture2D Texture1;

sampler TextureSampler1 = sampler_state { texture = <Texture1>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; }; Texture xShadowMap;

Texture2D ShadowMap;
sampler ShadowMapSampler = sampler_state { texture = <ShadowMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = clamp; AddressV = clamp; };

//POINT LIGHT

struct VertexToPixel
{
    float4 Position     : POSITION;
    float2 TexCoords    : TEXCOORD0;
    float3 Normal        : TEXCOORD1;
    float3 Position3D    : TEXCOORD2;
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

VertexToPixel SimplestVertexShader(float4 inPos : POSITION0, float3 inNormal : NORMAL0, float2 inTexCoords : TEXCOORD0)
{
    VertexToPixel Output = (VertexToPixel)0;

    Output.Position = mul(inPos, WorldViewProjection);
    Output.TexCoords = inTexCoords;
    Output.Normal = normalize(mul(inNormal, (float3x3)World));
    Output.Position3D = mul(inPos, World);

    return Output;
}

PixelToFrame OurFirstPixelShader(VertexToPixel PSIn)
{
    PixelToFrame Output = (PixelToFrame)0;

    float diffuseLightingFactor = DotProduct(LightPos, PSIn.Position3D, PSIn.Normal);
    diffuseLightingFactor = saturate(diffuseLightingFactor);
    diffuseLightingFactor *= LightPower;

    PSIn.TexCoords.y--;
    float4 baseColor = tex2D(TextureSampler1, PSIn.TexCoords);
    Output.Color = lerp(baseColor, float4(1, 1, 1, 1), (diffuseLightingFactor + Ambient));

    return Output;
}

technique BasicColorDrawing
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL SimplestVertexShader();
        PixelShader = compile PS_SHADERMODEL OurFirstPixelShader();
    }
}

//SHADOW MAP

struct CreateShadowMap_VSOut
{
    float4 Position : POSITION;
    float Depth : TEXCOORD0;
};

//  CREATE SHADOW MAP
CreateShadowMap_VSOut CreateShadowMap_VertexShader(float4 Position : SV_POSITION)
{
    CreateShadowMap_VSOut Out;
    Out.Position = mul(Position, LightWorldViewProjection);
    Out.Depth = Out.Position.z / Out.Position.w;

    return Out;
}

float4 CreateShadowMap_PixelShader(CreateShadowMap_VSOut input) : COLOR
{
    return float4(input.Depth, 0, 0, 0);
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
    float4 Position             : POSITION;
    float4 Pos2DAsSeenByLight    : TEXCOORD0;

    float2 TexCoords            : TEXCOORD1;
    float3 Normal                : NORMAL0;
    float4 Position3D            : TEXCOORD3;

};

struct SScenePixelToFrame
{
    float4 Color : COLOR0;
};


SSceneVertexToPixel ShadowedSceneVertexShader(float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0, float3 inNormal : NORMAL)
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
    if ((saturate(ProjectedTexCoords).x == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords).y == ProjectedTexCoords.y))
    {
        float depthStoredInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords).r;
        float realDistance = PSIn.Pos2DAsSeenByLight.z / PSIn.Pos2DAsSeenByLight.w;
        if ((realDistance - 1.0f / 100.0f) <= depthStoredInShadowMap)
        {
            diffuseLightingFactor = DotProduct(LightPos, PSIn.Position3D, PSIn.Normal);
            diffuseLightingFactor = saturate(diffuseLightingFactor);
            diffuseLightingFactor *= LightPower;
        }
    }

    float4 baseColor = tex2D(TextureSampler1, PSIn.TexCoords);
    Output.Color.rgb = baseColor.rgb * saturate(diffuseLightingFactor + Ambient);
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