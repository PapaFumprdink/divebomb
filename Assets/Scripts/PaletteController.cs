using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PaletteController : MonoBehaviour
{
    [SerializeField] private float m_TranstitionDuration;
    [SerializeField] private PostProcessVolume m_VolumeController;
    
    private float m_LastChangeTime;

    private void Update()
    {
        PostProcessProfile profile = m_VolumeController.profile;
        if (profile.TryGetSettings(out Palette pallete))
        {
            pallete.m_PaletteBlend.value = 1f - Mathf.Clamp01((Time.unscaledTime - m_LastChangeTime) /m_TranstitionDuration);
        }
    }

    public void SetPalette (Texture2D newPallete)
    {
        PostProcessProfile profile = m_VolumeController.profile;
        if (profile.TryGetSettings(out Palette pallete))
        {
            pallete.m_OldPaletteTexture.value = pallete.m_PaletteTexture;
            pallete.m_PaletteTexture.value = newPallete;
            m_LastChangeTime = Time.unscaledTime;
        }
    }
}
