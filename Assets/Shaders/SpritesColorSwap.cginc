// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

#ifndef UNITY_SPRITES_INCLUDED
#define UNITY_SPRITES_INCLUDED

#include "UnityCG.cginc"

#ifdef UNITY_INSTANCING_ENABLED

    UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
        // SpriteRenderer.Color while Non-Batched/Instanced.
        UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
        // this could be smaller but that's how bit each entry is regardless of type
        UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
    UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

    #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
    #define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

#endif // instancing

CBUFFER_START(UnityPerDrawSprite)
#ifndef UNITY_INSTANCING_ENABLED
    fixed4 _RendererColor;
    fixed2 _Flip;
#endif
    float _EnableExternalAlpha;
CBUFFER_END

// Material Color.
fixed4 _Color;

struct appdata_t
{
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 vertex   : SV_POSITION;
    fixed4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
{
    return float4(pos.xy * flip, pos.z, 1.0);
}

v2f SpriteVert(appdata_t IN)
{
    v2f OUT;

    UNITY_SETUP_INSTANCE_ID (IN);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

    OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
    OUT.vertex = UnityObjectToClipPos(OUT.vertex);
    OUT.texcoord = IN.texcoord;
    OUT.color = IN.color * _Color * _RendererColor;

    #ifdef PIXELSNAP_ON
    OUT.vertex = UnityPixelSnap (OUT.vertex);
    #endif

    return OUT;
}

sampler2D _MainTex;
sampler2D _AlphaTex;
float _HueShift;

fixed4 SampleSpriteTexture (float2 uv)
{
    fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
    fixed4 alpha = tex2D (_AlphaTex, uv);
    color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif

    return color;
}

half3 rgb2hsv(half3 c)
{
    half r = c.r;
    half g = c.g;
    half b = c.b;

    half maxVal = max(r, max(g, b));
    half minVal = min(r, min(g, b));
    half delta = maxVal - minVal;

    half h = 0.0;
    if (delta > 0.00001)
    {
        if (maxVal == r)
            h = (g - b) / delta + (g < b ? 6.0 : 0.0);
        else if (maxVal == g)
            h = (b - r) / delta + 2.0;
        else
            h = (r - g) / delta + 4.0;

        h /= 6.0;
    }

    half s = (maxVal <= 0.0) ? 0.0 : delta / maxVal;
    half v = maxVal;

    return half3(h, s, v);
}

half3 hsv2rgb(half3 c)
{
    half h = c.x;
    half s = c.y;
    half v = c.z;

    half r = v;
    half g = v;
    half b = v;

    if (s > 0.0)
    {
        h *= 6.0;
        int i = (int) h;
        half f = h - i;
        half p = v * (1.0 - s);
        half q = v * (1.0 - s * f);
        half t = v * (1.0 - s * (1.0 - f));

        if (i == 0)
        {
            r = v;
            g = t;
            b = p;
        }
        else if (i == 1)
        {
            r = q;
            g = v;
            b = p;
        }
        else if (i == 2)
        {
            r = p;
            g = v;
            b = t;
        }
        else if (i == 3)
        {
            r = p;
            g = q;
            b = v;
        }
        else if (i == 4)
        {
            r = t;
            g = p;
            b = v;
        }
        else
        {
            r = v;
            g = p;
            b = q;
        }
    }

    return half3(r, g, b);
}


fixed4 SpriteFrag(v2f IN) : SV_Target
{
    fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
    clip(c.a - 0.1);
    
    half3 hsv = rgb2hsv(c.rgb);
    hsv.x = fmod(hsv.x + _HueShift, 1.0);
    if (hsv.x < 0.0)
    {
        hsv.x += 1.0;
    }
    half3 rgb = hsv2rgb(hsv);
    return half4(rgb, c.a);
}

#endif // UNITY_SPRITES_INCLUDED
