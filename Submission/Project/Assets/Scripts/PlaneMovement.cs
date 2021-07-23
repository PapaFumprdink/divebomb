using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlaneMovement : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_MovementAcceleration;

    [Space]
    [SerializeField] private float m_TurnSpeed;
    [SerializeField] private AnimationCurve m_InputCurve;

    [Space]
    [SerializeField] private float m_ThrustDrag;
    [SerializeField] private float m_FreefallDrag;
    [SerializeField] private float m_FlapsDrag;

    [Space]
    [SerializeField] private float m_AfterburnerPower;

    [Space]
    [SerializeField] private UnityEvent m_EnginesCutEvent;
    [SerializeField] private UnityEvent m_EnginesReignitedEvent;

    private IMovementProvider m_MovementProvider;
    private Rigidbody2D m_Rigidbody;

    private bool m_EnginesWhereCut;

    private void Awake()
    {
        // Get the requied components.
        m_MovementProvider = GetComponent<IMovementProvider>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyTurning();
        SetRigidbodyProperties();
    }

    private void ApplyMovement()
    {
        if (!m_MovementProvider.EnginesCut)
        {
            // If the engines just reignited, call the appropriate event and apply the afterburner force.
            if (m_EnginesWhereCut)
            {
                m_EnginesReignitedEvent?.Invoke();
                m_Rigidbody.velocity = transform.up * m_AfterburnerPower;
            }

            // accelerate up to or down to the target speed with the acceleration.
            float normalizedSpeed = 1f - Mathf.Clamp01(m_Rigidbody.velocity.magnitude / m_MovementSpeed);
            float acceleration = m_MovementAcceleration * normalizedSpeed;
            m_Rigidbody.velocity += (Vector2)transform.up * acceleration * Time.deltaTime;
        }
        else if (!m_EnginesWhereCut)
        {
            // If the engines were just cut, call the appropriate event.
            m_EnginesCutEvent?.Invoke();
        }

        // Store the current engines state.
        m_EnginesWhereCut = m_MovementProvider.EnginesCut;
    }

    private void ApplyTurning()
    {
        // set the rigibodies acceleration. 
        float acceleration = m_MovementProvider.Steering * m_TurnSpeed;
        m_Rigidbody.angularVelocity = acceleration;
    }

    private void SetRigidbodyProperties()
    {
        // Set the rigidbodies properties based on whether the engines are cut or not.
        m_Rigidbody.drag = m_MovementProvider.EnginesCut ? m_FreefallDrag : m_ThrustDrag;
        m_Rigidbody.gravityScale = m_MovementProvider.EnginesCut ? 1f : 0f;
    }
}
