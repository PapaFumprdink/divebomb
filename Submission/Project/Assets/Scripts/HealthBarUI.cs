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
    [SerializeField] private float m_DifferenceFillSmoothing;

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
        }
    }
}
