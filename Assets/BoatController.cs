using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class BoatController : EnemyBase, IMovementProvider
{
    [SerializeField] private float m_AttackRange;
    [SerializeField] private Transform m_WeaponContainer;

    public bool EnginesCut => false;
    public float Steering { get; private set; }

    protected override void Update()
    {
        base.Update();

        if (CurrentTarget)
        {
            Steering = -Mathf.Sign(CurrentTarget.transform.position.x - transform.position.x);

            if ((CurrentTarget.transform.position - transform.position).sqrMagnitude < m_AttackRange * m_AttackRange)
            {
                Fire(0, false);
            }

            Vector2 direction = (CurrentTarget.transform.position - m_WeaponContainer.position).normalized;
            m_WeaponContainer.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_AttackRange);
        Gizmos.color = Color.white;
    }
}
