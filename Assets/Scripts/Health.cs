using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public class Health : MonoBehaviour, IDamagable
{
    public event DamageAction DamageEvent;
    public event DamageAction DeathEvent;

    [SerializeField] private float m_CurrentHealth;
    [SerializeField] private float m_MaxHealth;

    [Space]
    [SerializeField] private GameObject m_DestructionFX;

    public float CurrentHealth => m_CurrentHealth;
    public float MaxHealth => m_MaxHealth;
    public float NormalizedHealth => Mathf.Clamp01(CurrentHealth / MaxHealth);

    private void OnEnable()
    {
        // Reset current health.
        m_CurrentHealth = m_MaxHealth;

        // Disable destruction fx.
        if (m_DestructionFX)
        {
            m_DestructionFX.SetActive(false);
            m_DestructionFX.transform.parent = transform;
            m_DestructionFX.transform.localPosition = Vector3.zero;
            m_DestructionFX.transform.localRotation = Quaternion.identity;
        }
    }

    public void Damage(float damage, GameObject damager, Vector2 point, Vector2 direction)
    {
        // Decrement the health.
        m_CurrentHealth -= damage;

        // Call the damage event
        DamageEvent?.Invoke(damage, damager, point, direction);

        // If our health drops below 0, kill the object.
        if (m_CurrentHealth < 0)
        {
            Kill(damage, damager, point, direction);
        }
    }

    public void Kill(float damage, GameObject killer, Vector2 point, Vector2 direction)
    {
        // Display the destruction fx.
        if (m_DestructionFX)
        {
            m_DestructionFX.SetActive(true);
            m_DestructionFX.transform.parent = null;
        }

        // Call the event and disable the gameObject as to not break references.
        DeathEvent?.Invoke(damage, killer, point, direction);
        gameObject.SetActive(false);
    }
}
