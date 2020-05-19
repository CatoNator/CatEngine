#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

matrix World;
matrix WorldViewProjection;
matrix WorldInverseTranspose;
matrix LightWorldViewProjection;
float4x4 gBonesOffsets[50];
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;
float3 CameraPos;
float3 LightPos;
float LightPower;
float Ambient;
float3 SunOrientation;

Texture2D Texture1;
sampler TextureSampler1 =
sampler_state
{
    Texture = <Texture1>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

Texture2D ShadowMap;
sampler ShadowMapSampler = sampler_state
{
    texture = <ShadowMap>;
    AddressU = clamp;
    AddressV = clamp;
};

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);
}

struct VertexShaderInput
{
    float4 Position : SV_POSITION;
    float3 Normal : NORMAL0;
    float2 Uv : TEXCOORD0;
    float4 blendIndices : BLENDINDICES;
    float4 blendWeights : BLENDWEIGHT;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float3 Normal : NORMAL0;
    float2 Uv : TEXCOORD0;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    float4 skinnedPosition = float4(0.0, 0.0, 0.0, 0.0);
    float4 skinnedNormal = float4(0.0, 0.0, 0.0, 0.0);

    int index = input.blendIndices[0];
    skinnedPosition += mul(input.Position, gBonesOffsets[index]) * input.blendWeights[0];

    index = input.blendIndices[1];
    skinnedPosition += mul(input.Position, gBonesOffsets[index]) * input.blendWeights[1];

    index = input.blendIndices[2];
    skinnedPosition += mul(input.Position, gBonesOffsets[index]) * input.blendWeights[2];

    index = input.blendIndices[3];
    skinnedPosition += mul(input.Position, gBonesOffsets[index]) * input.blendWeights[3];

    //skinnedNormal += mul(float4(input.Normal, 0.0), gBonesOffsets[index]) * weight;


    output.Position = mul(skinnedPosition, WorldViewProjection);
    output.Normal = mul(input.Normal, World);
    output.Uv = input.Uv;

    float4 normal = mul(input.Normal, WorldInverseTranspose);
    float lightIntensity = dot(normal, SunOrientation);
    output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float cosTheta = clamp(dot(input.Normal, SunOrientation) + 1, 0, 1);
    float4 textureColor = tex2D(TextureSampler1, input.Uv);// input.Color * cosTheta;
    textureColor.a = 1;
    return textureColor;
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};

//------- Technique: ShadowMap --------

struct SMapVertexToPixel
{
    float4 Position     : POSITION;
    float4 PositionForPS    : TEXCOORD0;
};

struct SMapPixelToFrame
{
    float4 Color : COLOR0;
};

SMapVertexToPixel ShadowMapVertexShader(float4 inPos : POSITION)
{
    SMapVertexToPixel Output = (SMapVertexToPixel)0;

    Output.Position = mul(inPos, LightWorldViewProjection);
    Output.PositionForPS = Output.Position; // make a copy to send to pixel shader

    return Output;
}

SMapPixelToFrame ShadowMapPixelShader(SMapVertexToPixel PSIn)
{
    SMapPixelToFrame Output = (SMapPixelToFrame)0;

    Output.Color = PSIn.PositionForPS.z / PSIn.PositionForPS.w;

    return Output;
}

technique ShadowMap
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL ShadowMapVertexShader();
        PixelShader = compile PS_SHADERMODEL ShadowMapPixelShader();
    }
}

//------- Technique: ShadowedScene --------

struct SSceneVertexToPixel
{
    float4 Position             : POSITION;
    float4 Pos2DAsSeenByLight    : TEXCOORD0;

    float2 TexCoords            : TEXCOORD1;
    float3 Normal                : TEXCOORD2;
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
    float4 shadowColor = float4(0, 0, 0, 0);
    Output.Color = lerp(shadowColor, baseColor, (diffuseLightingFactor + Ambient));

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