using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class OutlineManager
{
    private static List<OutlineFX> m_OutlineEffects = new List<OutlineFX>();

    public static void Register (OutlineFX outlineFX)
    {
        m_OutlineEffects.Add(outlineFX);
    }

    public static void Unregister(OutlineFX outlineFX)
    {
        m_OutlineEffects.RemoveAll(q => q == outlineFX);
    }

    public static void PopulateCommandBuffer (CommandBuffer commandBuffer)
    {
        foreach (OutlineFX fx in m_OutlineEffects)
        {
            commandBuffer.DrawRenderer(fx.Renderer, fx.Renderer.sharedMaterial);
        }
    }
}
