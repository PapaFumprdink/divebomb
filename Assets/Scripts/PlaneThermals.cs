using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneThermals : MonoBehaviour, IWaterInterator
{
    [SerializeField] private float m_MaxHeat;
    [SerializeField] private float m_CurrentHeat;
    [SerializeField] private AnimationCurve m_Decay;
    [SerializeField] private AnimationCurve m_HeatDamage;
    [SerializeField] private float m_DamageFrequency;

    [Space]
    [SerializeField] private float m_WaterHeatDecay;

    private IDamagable m_DamageComponent;
    private float m_NextDamageTime;
    private bool m_IsInWater;

    public float MaxHeat => m_MaxHeat;
    public float CurrentHeat => m_CurrentHeat;
    public float NormalizedHeat => Mathf.Clamp01(CurrentHeat / MaxHeat);

    private void Awake()
    {
        // Store the damage component for performance.
        m_DamageComponent = GetComponent<IDamagable>();
    }

    public void AddHeat (float heat)
    {
        // add the current heat with a clamp to stop overflow.
        m_CurrentHeat = Mathf.Clamp(m_CurrentHeat + heat, 0f, m_MaxHeat);
    }

    private void Update()
    {
        // Store the normalized heat for brevity.
        float normalizedHeat = m_CurrentHeat / m_MaxHeat;

        // Decay the heat based on the decay curve.
        float heatDecay = m_Decay.Evaluate(normalizedHeat) * Time.deltaTime;
        if (m_IsInWater) heatDecay *= m_WaterHeatDecay;
        m_CurrentHeat = Mathf.Clamp(m_CurrentHeat - heatDecay, 0f, m_MaxHeat);

        // Calculate the damage applied from the heat. Only apply if over 0 and if enough time has passed since the last damage time to stop any unnecessary calls.
        float heatDamage = m_HeatDamage.Evaluate(normalizedHeat);
        if (heatDamage > 0f && Time.time > m_NextDamageTime)
        {
            m_DamageComponent?.Damage(m_HeatDamage.Evaluate(normalizedHeat) / m_DamageFrequency, gameObject, transform.position, Vector2.zero);
            m_NextDamageTime = Time.time + 1f / m_DamageFrequency;
        }
    }

    public void EnterWater()
    {
        m_IsInWater = true;
        m_CurrentHeat = 0f;
    }

    public void ExitWater()
    {
        m_IsInWater = false;
    }
}
