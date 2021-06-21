using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PaletteController : MonoBehaviour
{
    public static Texture2D CurrentPalette;
    public static Texture2D OldPallete;

    [SerializeField] private float m_TranstitionDuration;
    [SerializeField] private PostProcessVolume m_VolumeController;
    
    private float m_LastChangeTime;

    private void Start()
    {
        PostProcessProfile profile = m_VolumeController.profile;
        if (profile.TryGetSettings(out Palette pallete))
        {
            if (OldPallete) pallete.m_OldPaletteTexture.value = OldPallete;
            if (CurrentPalette) pallete.m_PaletteTexture.value = CurrentPalette;
        }
    }

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
        OldPallete = CurrentPalette;
        CurrentPalette = newPallete;

        PostProcessProfile profile = m_VolumeController.profile;
        if (profile.TryGetSettings(out Palette pallete))
        {
            pallete.m_OldPaletteTexture.value = OldPallete;
            pallete.m_PaletteTexture.value = CurrentPalette;
            m_LastChangeTime = Time.unscaledTime;
        }
    }
}
