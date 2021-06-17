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
    [SerializeField] private float m_RotationalSmoothing;

    private Rigidbody2D m_Rigidbody;
    private bool m_IsStationary;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!CurrentTarget)
        {
            // If we have no target, find one
            CurrentTarget = PlayerController.PlayerInstances[Random.Range(0, PlayerController.PlayerInstances.Count)].gameObject;
        }
        else
        {
            Vector2 vectorToTarget = (CurrentTarget.transform.position - transform.position);

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
                m_Rigidbody.velocity -= (m_MovementSpeed / m_AccelerationTime) * Time.deltaTime * m_Rigidbody.velocity;
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
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0f, (facingDirection + 1) * 90f, 0f), transform.rotation, m_RotationalSmoothing * Time.deltaTime);
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
