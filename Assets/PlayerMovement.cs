using System;
using UnityEngine;

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

    private PlayerController m_PlayerController;
    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        m_PlayerController = GetComponent<PlayerController>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyTurning();
    }

    private void ApplyMovement()
    {
        float normalizedSpeed = 1f - Mathf.Clamp01(m_Rigidbody.velocity.magnitude / m_MovementSpeed);
        float acceleration = m_MovementAcceleration * normalizedSpeed;
        m_Rigidbody.velocity += (Vector2)transform.up * acceleration * Time.deltaTime;
    }

    private void ApplyTurning()
    {
        float acceleration = m_InputCurve.Evaluate(Mathf.Abs(m_PlayerController.Steering)) * Mathf.Sign(m_PlayerController.Steering) * m_TurnSpeed;
        m_Rigidbody.angularVelocity = acceleration;
    }
}
