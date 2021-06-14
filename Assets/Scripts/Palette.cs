using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PaletteRenderer), PostProcessEvent.AfterStack, "Custom/Palette")]
public class Palette : PostProcessEffectSettings
{
    public TextureParameter m_PaletteTexture = new TextureParameter();
    public TextureParameter m_OldPaletteTexture = new TextureParameter();
    public TextureParameter m_BlendMask = new TextureParameter();
    [Range(0f, 1f)]
    public FloatParameter m_PaletteBlend = new FloatParameter();
}

public sealed class PaletteRenderer : PostProcessEffectRenderer<Palette>
{
    public override void Render(PostProcessRenderContext context)
    {
        // Get the property sheet
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/PostProcess/Palette"));

        // Set all of the parameters on the sheet
        if (settings.m_PaletteTexture.value) sheet.properties.SetTexture("_Palette", settings.m_PaletteTexture);
        if (settings.m_OldPaletteTexture.value) sheet.properties.SetTexture("_OldPalette", settings.m_OldPaletteTexture);
        if (settings.m_BlendMask.value) sheet.properties.SetTexture("_BlendMask", settings.m_BlendMask);
        sheet.properties.SetFloat("_Blend", settings.m_PaletteBlend);

        // Apply the shader to the fullscreen tri
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}