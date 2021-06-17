using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class WeaponGuage : MonoBehaviour
{
    [SerializeField] private Weapon m_Target;
    [SerializeField] private Image m_FillElement;

    [Space]
    [SerializeField] private AnimationCurve m_CooldownFinishScale;

    private float m_LastCooldownFinishTime;
    private float m_PreviousCooldown;

    private void Update()
    {
        // Set the bars fill.
        m_FillElement.fillAmount = 1f - m_Target.NormalizedCooldown;

        if (m_Target.NormalizedCooldown < 0.01f && m_PreviousCooldown >= 0.01f)
        {
            m_LastCooldownFinishTime = Time.time;
        }

        m_FillElement.transform.localScale = Vector3.one * m_CooldownFinishScale.Evaluate(Time.time - m_LastCooldownFinishTime);

        m_PreviousCooldown = m_Target.NormalizedCooldown;
    }
}
