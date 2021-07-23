using System;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class Mine : MonoBehaviour, IDamagable
{
    [SerializeField] private float m_Damage;
    [SerializeField] private float m_DamageRange;
    [SerializeField] private float m_ArmTime;
    [SerializeField] private float m_FuzeTime;
    [SerializeField] private Transform m_ExplosionFX;

    private float m_SpawnTime;
    private bool m_HasDetonated;

    public float CurrentHealth => enabled ? 1f : 0f;

    public float MaxHealth => 1f;

    public float NormalizedHealth => CurrentHealth;

    public event DamageAction DamageEvent;
    public event DamageAction DeathEvent;

    private void Awake()
    {
        m_SpawnTime = Time.time;
    }

    private void OnEnable()
    {
        // Reset component to stop any errors.
        m_HasDetonated = false;

        m_ExplosionFX.gameObject.SetActive(false);
        m_ExplosionFX.parent = transform;
        m_ExplosionFX.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are using a trigger range, detonate when there is something in the trigger.
        Detonate();
    }

    private void Update()
    {
        // If our fuze has run out, detonate
        if (Time.time > m_SpawnTime + m_FuzeTime && !m_HasDetonated)
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        // If the arm time has passed and we have not already detonated (to stop stack
        // overflow between two bombs exploding each other)
        if (Time.time > m_SpawnTime + m_ArmTime && !m_HasDetonated)
        {
            // Set the detonation status before damage calc to stop stack overflow.
            m_HasDetonated = true;

            foreach (Collider2D query in Physics2D.OverlapCircleAll(transform.position, m_DamageRange))
            {
                // If we are not detonating ourselves.
                if (query.gameObject != gameObject)
                {
                    if (query.TryGetComponent(out IDamagable damagable))
                    {
                        damagable.Damage(m_Damage, gameObject, transform.position, query.transform.position - transform.position);
                    }
                }
            }

            // Activate and detach the fx.
            m_ExplosionFX.gameObject.SetActive(true);
            m_ExplosionFX.parent = null;

            Destroy(gameObject);
        }
    }

    public void HealOneShot(float health, GameObject healer) { }

    public void Damage(float damage, GameObject damager, Vector2 point, Vector2 direction)
    {
        DamageEvent?.Invoke(damage, damager, point, direction);
        DeathEvent?.Invoke(damage, damager, point, direction);
        Detonate();
    }

    public void Kill(float damage, GameObject killer, Vector2 point, Vector2 direction)
    {
        Damage(damage, killer, point, direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_DamageRange);
        Gizmos.color = Color.white;
    }
}
