#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

const float amount = 400;
float spread = 0.08;
float redcount = 12;
float greencount = 12;
float bluecount = 12;

int bayer4[4 * 4] =
{
    0, 8, 2, 10,
    12, 4, 14, 6,
    3, 11, 1, 9,
    15, 7, 13, 5
};

float GetBayer4(int x, int y)
{
    return float(bayer4[(x % 4) + (y % 4) * 4]) * (1.0f / 16.0f) - 0.5f;
}

texture ScreenTexture;
sampler2D TextureSampler : register(s0)
{
    Texture = (ScreenTexture);
};

struct VertexShaderOutput
{
    float4 pos : PS_POSITION;
    float2 txc : TEXCOORD0;
};

float4 MainPS(in float2 txc : TEXCOORD0) : COLOR
{
    float2 coord = txc * 0.2;
    float4 tex = tex2D(TextureSampler, coord);

    //float coordx = txc.x * 0.6;
    //float coordy = txc.y * 0.6;

    //int x = int(coordx * 1280);
    //int y = int(coordy * 880);
    
    float4 final = tex;
    //float4 final = tex + spread * GetBayer4(x, y);
    //final.x = floor((redcount - 1.0) * final.r + 0.5) / (redcount - 1.0);
    //final.y = floor((greencount - 1.0) * final.g + 0.5) / (greencount - 1.0);
    //final.z = floor((bluecount - 1.0) * final.b + 0.5) / (bluecount - 1.0);
    
    return final;
}

technique BasicColorDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};