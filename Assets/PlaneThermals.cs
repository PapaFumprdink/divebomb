using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneThermals : MonoBehaviour
{
    [SerializeField] private float m_MaxHeat;
    [SerializeField] private float m_CurrentHeat;
    [SerializeField] private AnimationCurve m_Decay;
    [SerializeField] private AnimationCurve m_HeatDamage;
    [SerializeField] private float m_DamageFrequency;

    private IDamagable m_DamageComponent;
    private float m_NextDamageTime;

    public float MaxHeat => m_MaxHeat;
    public float CurrentHeat => m_CurrentHeat;

    private void Awake()
    {
        m_DamageComponent = GetComponent<IDamagable>();
    }

    public void AddHeat (float heat)
    {
        m_CurrentHeat = Mathf.Clamp(m_CurrentHeat + heat, 0f, m_MaxHeat);
    }

    private void Update()
    {
        float normalizedHeat = m_CurrentHeat / m_MaxHeat;
        m_CurrentHeat = Mathf.Clamp(m_CurrentHeat - m_Decay.Evaluate(normalizedHeat) * Time.deltaTime, 0f, m_MaxHeat);

        float heatDamage = m_HeatDamage.Evaluate(normalizedHeat);
        if (heatDamage > 0f && Time.time > m_NextDamageTime)
        {
            m_DamageComponent?.Damage(m_HeatDamage.Evaluate(normalizedHeat) / m_DamageFrequency, gameObject, transform.position, Vector2.zero);
            m_NextDamageTime = Time.time + 1f / m_DamageFrequency;
        }
    }
}
