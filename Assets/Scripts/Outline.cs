using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(OutlineRenderer), PostProcessEvent.BeforeStack, "Custom/Outline")]
public sealed class Outline : PostProcessEffectSettings
{
    public ColorParameter m_OutlineColor = new ColorParameter();
    public FloatParameter m_OutlineSize = new FloatParameter();
}

public sealed class OutlineRenderer : PostProcessEffectRenderer<Outline>
{
    private int m_GlobalOutlineTextureID;

    public override void Init()
    {
        m_GlobalOutlineTextureID = Shader.PropertyToID("_GlobalOutlineTexture");

        base.Init();
    }

    public override void Render(PostProcessRenderContext context)
    {
        // Get the property sheet
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/PostProcess/Outline"));
       
        // Set all of the parameters on the sheet
        sheet.properties.SetFloat("_OutlineSize", settings.m_OutlineSize);
        sheet.properties.SetColor("_OutlineColor", settings.m_OutlineColor);

        context.command.GetTemporaryRT(m_GlobalOutlineTextureID,
            context.camera.pixelWidth,
            context.camera.pixelHeight,
            0, FilterMode.Bilinear, RenderTextureFormat.Default);
        context.command.SetRenderTarget(m_GlobalOutlineTextureID);
        context.command.ClearRenderTarget(false, true, Color.clear);

        // Apply the shader to the fullscreen tri
        OutlineManager.PopulateCommandBuffer(context.command);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
