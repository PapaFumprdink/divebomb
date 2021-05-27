using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public class PlaneHealth : MonoBehaviour, IDamagable
{
    public event DamageAction DamageEvent;
    public event DamageAction DeathEvent;

    [SerializeField] private float m_CurrentHealth;
    [SerializeField] private float m_MaxHealth;

    public float CurrentHealth => m_CurrentHealth;
    public float MaxHealth => m_MaxHealth;
    public float NormalizedHealth => Mathf.Clamp01(CurrentHealth / MaxHealth);

    private void OnEnable()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void Damage(float damage, GameObject damager, Vector2 point, Vector2 direction)
    {
        m_CurrentHealth -= damage;

        DamageEvent?.Invoke(damage, damager, point, direction);

        if (m_CurrentHealth < 0)
        {
            Kill(damage, damager, point, direction);
        }
    }

    public void Kill(float damage, GameObject killer, Vector2 point, Vector2 direction)
    {
        DeathEvent?.Invoke(damage, killer, point, direction);
        gameObject.SetActive(false);
    }
}
