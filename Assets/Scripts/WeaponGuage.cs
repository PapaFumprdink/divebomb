using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class WeaponGuage : MonoBehaviour
{
    [SerializeField] private Weapon m_Target;
    [SerializeField] private Image m_FillElement;

    private void Update()
    {
        // Set the bars fill.
        m_FillElement.fillAmount = 1f - m_Target.NormalizedCooldown;
    }
}
