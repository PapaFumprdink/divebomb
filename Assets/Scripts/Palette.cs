using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PaletteRenderer), PostProcessEvent.AfterStack, "Custom/Palette")]
public class Palette : PostProcessEffectSettings
{
    public TextureParameter m_PaletteTexture = new TextureParameter();
}

public sealed class PaletteRenderer : PostProcessEffectRenderer<Palette>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/PostProcess/Palette"));
        if (settings.m_PaletteTexture.value) sheet.properties.SetTexture("_Palette", settings.m_PaletteTexture);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}