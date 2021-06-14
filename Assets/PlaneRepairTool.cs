using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlaneRepairTool : MonoBehaviour
{
    [SerializeField] private float m_RepairFrequency;
    [SerializeField] private float m_RepairAmount;

    [Space]
    [SerializeField] private CursorObject m_CursorObject;
    [SerializeField] private Sprite m_RepairCursorImage;
    [SerializeField] private AnimationCurve m_VisualProgressCurve;

    private IDamagable m_HealthComponent;
    private float m_RepairProgress;

    private PlayerController m_Controller;

    private void Awake()
    {
        m_Controller = GetComponent<PlayerController>();
        m_HealthComponent = GetComponent<IDamagable>();
    }

    private void Update()
    {
        if (m_Controller.Repairing)
        {
            m_RepairProgress += Time.deltaTime / m_RepairFrequency;

            m_CursorObject.FillProgress = m_VisualProgressCurve.Evaluate(m_RepairProgress);
            m_CursorObject.CursorSprite = m_RepairCursorImage;
        }
        else
        {
            m_RepairProgress = 0f;
        }

        if (m_RepairProgress > 1f)
        {
            m_HealthComponent.HealOneShot(m_RepairAmount, gameObject);
            m_RepairProgress = 0f;
        }
    }
}
