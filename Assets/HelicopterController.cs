using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class HelicopterController : EnemyBase
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_AccelerationTime;
    [SerializeField] private float m_DistanceFromTarget;
    [SerializeField] private float m_FireRange;
    [SerializeField] private float m_RotationalSmoothing;

    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!CurrentTarget)
        {
            CurrentTarget = PlayerController.PlayerInstances[Random.Range(0, PlayerController.PlayerInstances.Count)].gameObject;
        }
        else
        {
            Vector2 vectorToTarget = CurrentTarget.transform.position - transform.position;
            float facingDirection = Mathf.Sign(vectorToTarget.x);
            Vector2 targetPosition = CurrentTarget.transform.position + Vector3.left * facingDirection * m_DistanceFromTarget;
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

            Vector2 currentVelocity = m_Rigidbody.velocity;
            Vector2 targetVelocity = direction * m_MovementSpeed;
            Vector2 acceleration = Vector2.ClampMagnitude(targetVelocity - currentVelocity, m_MovementSpeed) / m_AccelerationTime;

            m_Rigidbody.velocity += acceleration * Time.deltaTime;

            if (Mathf.Abs(vectorToTarget.y) < m_FireRange)
            {
                Fire(0, false);
            }

            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0f, (facingDirection + 1) * 90f, 0f), transform.rotation, m_RotationalSmoothing * Time.deltaTime);
        }
    }
}
