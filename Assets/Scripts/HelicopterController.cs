using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class HelicopterController : EnemyBase
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_AccelerationTime;
    [SerializeField] private float m_IdleRange;
    [SerializeField] private float m_FollowRange;
    [SerializeField] private float m_FleeRange;
    [SerializeField] private float m_RotationalSmoothing;

    [Space]
    [SerializeField] private Transform m_Model;

    private Rigidbody2D m_Rigidbody;
    private bool m_IsStationary;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (CurrentTarget)
        {
            Vector2 vectorToTarget = (CurrentTarget.transform.position - transform.position);
            ProcessMovementDirection(ref vectorToTarget);

            if (vectorToTarget.sqrMagnitude > m_FollowRange * m_FollowRange)
            {
                m_IsStationary = false;
            }
            else if (vectorToTarget.sqrMagnitude < m_IdleRange * m_IdleRange)
            {
                m_IsStationary = true;
            }

            if (m_IsStationary)
            {
                if (vectorToTarget.sqrMagnitude < m_FleeRange * m_FleeRange)
                {
                    Vector2 currentVelocity = m_Rigidbody.velocity;
                    Vector2 targetVelocity = -vectorToTarget.normalized * m_MovementSpeed;
                    Vector2 acceleration = Vector2.ClampMagnitude(targetVelocity - currentVelocity, m_MovementSpeed) / m_AccelerationTime;

                    // Set the velocity.
                    m_Rigidbody.velocity += acceleration * Time.deltaTime;
                }
                else
                {
                    m_Rigidbody.velocity -= (m_MovementSpeed / m_AccelerationTime) * Time.deltaTime * m_Rigidbody.velocity;
                }
                Fire(0, false);
            }
            else
            {
                Vector2 currentVelocity = m_Rigidbody.velocity;
                Vector2 targetVelocity = vectorToTarget.normalized * m_MovementSpeed;
                Vector2 acceleration = Vector2.ClampMagnitude(targetVelocity - currentVelocity, m_MovementSpeed) / m_AccelerationTime;

                // Set the velocity.
                m_Rigidbody.velocity += acceleration * Time.deltaTime;
            }

            // Calculations for velocity application
            float facingDirection = Mathf.Sign(vectorToTarget.x);

            // Set rotation with smoothing.
            m_Model.rotation = Quaternion.Slerp(m_Model.rotation, Quaternion.Euler(0f, (facingDirection + 1) * 90f, 0f), m_RotationalSmoothing * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_IdleRange);
        Gizmos.DrawWireSphere(transform.position, m_FollowRange);
        Gizmos.color = Color.white;
    }
}
