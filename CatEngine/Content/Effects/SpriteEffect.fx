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
float4x4 Transform;
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
    magfilter = POINT;
    minfilter = POINT;
    mipfilter = LINEAR;
    AddressU = clamp;
    AddressV = clamp;
};

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

VertexToPixel BasicVertexShader(float4 Position : POSITION, float2 TexCoords : TEXCOORD0)
{
    VertexToPixel Out;
    Out.Position = mul(mul(Position, Transform), WorldViewProjection);
    Out.TexCoords = TexCoords;
    return Out;
}

PixelToFrame BasicPixelShader(VertexToPixel input)
{
    PixelToFrame Out;
    Out.Color = tex2D(TextureSampler1, input.TexCoords);
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