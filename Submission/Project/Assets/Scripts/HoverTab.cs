using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class HoverTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform m_InPoint;
    [SerializeField] private Transform m_OutPoint;
    [SerializeField] private Transform m_Tab;
    [SerializeField] private float m_TransitionTime;
    [SerializeField] private AnimationCurve m_TransitionCurve;

    private bool m_IsOut;
    private float m_Percent;

    private void Update()
    {
        // Lerp the menu position based on the percent.
        m_Percent = Mathf.Clamp01(m_Percent + Time.unscaledDeltaTime / (m_IsOut ? -m_TransitionTime : m_TransitionTime));
        m_Tab.position = Vector3.Lerp(m_InPoint.position, m_OutPoint.position, m_TransitionCurve.Evaluate(m_Percent));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_IsOut = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_IsOut = false;
    }
}
