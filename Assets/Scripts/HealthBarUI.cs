using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject m_TargetGameObject;

    [Space]
    [SerializeField] private Image m_Fill;
    [SerializeField] private Image m_DifferenceFill;
    [SerializeField] private Image m_Border;
    [SerializeField] private TMPro.TMP_Text m_Label;
    [SerializeField] private float m_DifferenceFillSmoothing;

    [Space]
    [SerializeField] private PostProcessProfile m_DefaultPostProcessProfile;

    private IDamagable m_Target;

    private void Awake()
    {
        m_Target = m_TargetGameObject.GetComponent<IDamagable>();
    }

    private void Update()
    {
        if (m_Target != null)
        {
            // Scale the bars fill to the normalized heat.
            m_Fill.fillAmount = m_Target.NormalizedHealth;

            // Set the difference fill to a smoothed out version of the health bar.
            m_DifferenceFill.fillAmount = Mathf.Lerp(m_DifferenceFill.fillAmount, m_Target.NormalizedHealth, m_DifferenceFillSmoothing * Time.deltaTime);

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
