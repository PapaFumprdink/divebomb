using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class HeatBarUI : MonoBehaviour
{
    [SerializeField] private PlaneThermals m_Target;

    [Space]
    [SerializeField] private Image m_Fill;
    [SerializeField] private Image m_DifferenceFill;
    [SerializeField] private Image m_Border;
    [SerializeField] private TMPro.TMP_Text m_Label;

    [Space]
    [SerializeField] private PostProcessProfile m_DefaultPostProcessProfile;

    private void Update()
    {
        if (m_Target)
        {
            // Scale the bars fill to the normalized heat.
            m_Fill.fillAmount = m_Target.NormalizedHeat;

            // Get the palette component 
            if (m_DefaultPostProcessProfile.TryGetSettings(out Palette palette))
            {
                // Get pallete tones
                palette.GetPaletteTones(out Color highTone, out Color midTone, out Color lowTone);

                // Set the visual components to the appropriate color.
                m_Fill.color = midTone;
                m_DifferenceFill.color = highTone;
                m_Border.color = highTone;
                m_Label.color = highTone;
            }
        }
    }
}
