using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class BoatMovement : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_AccelerationTime;
    [SerializeField] private float m_MaxRotationDelta;
    
    [Space]
    [SerializeField] private float m_WaterLevel;

    private IMovementProvider m_MovementProvider;
    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_MovementProvider = GetComponent<IMovementProvider>();
    }

    private void Update()
    {
        Vector2 currentVelocity = m_Rigidbody.velocity;
        Vector2 targetVelocity = transform.right * m_MovementSpeed;
        Vector2 force = Vector2.ClampMagnitude(targetVelocity - currentVelocity, m_MovementSpeed) / m_AccelerationTime;

        m_Rigidbody.velocity += force * Time.deltaTime;


        Quaternion targetRotation = Quaternion.Euler(0f, (m_MovementProvider.Steering + 1) * 90f, 0f);
        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angle * m_MaxRotationDelta * Time.deltaTime);


        transform.position = new Vector3(transform.position.x, m_WaterLevel);
    }
}
