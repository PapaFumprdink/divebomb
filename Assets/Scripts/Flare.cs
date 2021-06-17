using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class Flare : Projectile
{
    [SerializeField] private float m_InfluenceRange;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Loop through everything in range.
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, m_InfluenceRange))
        {
            // If anything in range has a targeting componet, reset the target to this.
            if (collider.TryGetComponent(out EnemyBase enemy))
            {
                enemy.CurrentTarget = gameObject;
            }

            if (collider.TryGetComponent(out ITargetable targetable))
            {
                targetable.Target = transform;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_InfluenceRange);
        Gizmos.color = Color.white;
    }
}
