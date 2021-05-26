using System;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_MovementAcceleration;

    [Space]
    [SerializeField] private float m_TurnSpeed;
    [SerializeField] private AnimationCurve m_InputCurve;

    [Space]
    [SerializeField] private float m_ThrustDrag;
    [SerializeField] private float m_FreefallDrag;

    [Space]
    [SerializeField] private float m_AfterburnerPower;

    [Space]
    [SerializeField] private UnityEvent m_EnginesCutEvent;
    [SerializeField] private UnityEvent m_EnginesReignitedEvent;

    private PlayerController m_PlayerController;
    private Rigidbody2D m_Rigidbody;

    private bool m_EnginesWhereCut;

    private void Awake()
    {
        m_PlayerController = GetComponent<PlayerController>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyTurning();
        SetRigidbodyProperties();
    }

    private void SetRigidbodyProperties()
    {
        m_Rigidbody.drag = m_PlayerController.EnginesCut ? m_FreefallDrag : m_ThrustDrag;
        m_Rigidbody.gravityScale = m_PlayerController.EnginesCut ? 1f : 0f;
    }

    private void ApplyMovement()
    {
        if (!m_PlayerController.EnginesCut)
        {
            if (m_EnginesWhereCut)
            {
                m_EnginesReignitedEvent?.Invoke();
                m_Rigidbody.velocity = transform.up * m_AfterburnerPower;
            }

            float normalizedSpeed = 1f - Mathf.Clamp01(m_Rigidbody.velocity.magnitude / m_MovementSpeed);
            float acceleration = m_MovementAcceleration * normalizedSpeed;
            m_Rigidbody.velocity += (Vector2)transform.up * acceleration * Time.deltaTime;
        }
        else if (!m_EnginesWhereCut)
        {
            m_EnginesCutEvent?.Invoke();
        }

        m_EnginesWhereCut = m_PlayerController.EnginesCut;
    }

    private void ApplyTurning()
    {
        float acceleration = m_InputCurve.Evaluate(Mathf.Abs(m_PlayerController.Steering)) * Mathf.Sign(m_PlayerController.Steering) * m_TurnSpeed;
        m_Rigidbody.angularVelocity = acceleration;
    }
}
