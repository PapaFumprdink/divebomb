using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class HomingProjectile : Projectile, ITargetable
{
    [SerializeField] private float m_HomingInfluence;
    [SerializeField] private LineRenderer m_TargetDisplay;

    public Transform Target { get; set; }

    protected override void FixedUpdate()
    {
        if (Target)
        {
            // Calculate the cross to target to drive the rotation.
            Vector2 directionToTarget = (Target.position - transform.position).normalized;
            float crossToTarget = Vector3.Cross(m_Rigidbody.velocity.normalized, directionToTarget).z;

            // Rotate the velocity, the base will set the rotation.
            m_Rigidbody.velocity = Quaternion.Euler(0f, 0f, crossToTarget * m_HomingInfluence * Time.deltaTime) * m_Rigidbody.velocity;

            // If we have a display component, draw a line to the target.
            if (m_TargetDisplay)
            {
                m_TargetDisplay.SetPosition(0, transform.position);
                m_TargetDisplay.SetPosition(1, Target.position);
                m_TargetDisplay.enabled = true;
            }
        }
        else if (m_TargetDisplay)
        {
            m_TargetDisplay.enabled = false;
        }

        base.FixedUpdate();
    }
}
