using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PixelateRenderer), PostProcessEvent.AfterStack, "Custom/Pixelate")]
public sealed class Pixelate : PostProcessEffectSettings
{
    public FloatParameter m_PixelsPerUnit = new FloatParameter();
}

public sealed class PixelateRenderer : PostProcessEffectRenderer<Pixelate>
{
    public override void Render(PostProcessRenderContext context)
    {
        // Get the property sheet
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/PostProcess/Pixelate"));
       
        // Set all of the parameters on the sheet
        sheet.properties.SetFloat("_PixelScale", settings.m_PixelsPerUnit);

        // Apply the shader to the fullscreen tri
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
