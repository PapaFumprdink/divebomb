using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(DistortionRenderer), PostProcessEvent.AfterStack, "Custom/Distortion")]
public sealed class Distortion : PostProcessEffectSettings
{
    public TextureParameter m_DistortionReference = new TextureParameter();
    public FloatParameter m_Strength = new FloatParameter();
}

public sealed class DistortionRenderer : PostProcessEffectRenderer<Distortion>
{
    public override void Render(PostProcessRenderContext context)
    {
        // Get the property sheet
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/PostProcess/Distortion"));

        // Set all of the parameters on the sheet
        if (settings.m_DistortionReference.value) sheet.properties.SetTexture("_Distortion", settings.m_DistortionReference);
        sheet.properties.SetFloat("_Strength", settings.m_Strength);

        // Apply the shader to the fullscreen tri
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}