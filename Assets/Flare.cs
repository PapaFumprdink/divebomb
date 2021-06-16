using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class Flare : Projectile
{
    [SerializeField] private float m_InfluenceRange;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, m_InfluenceRange))
        {
            if (collider.TryGetComponent(out EnemyChase enemy))
            {
                enemy.CurrentTarget = gameObject;
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
