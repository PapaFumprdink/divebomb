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

    private void Update()
    {
        if (m_Target)
        {
            // Scale the bars fill to the normalized heat.
            m_Fill.fillAmount = m_Target.NormalizedHeat;
        }
    }
}
