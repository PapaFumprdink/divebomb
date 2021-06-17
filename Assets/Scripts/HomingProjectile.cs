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
                for (int i = 0; i < m_TargetDisplay.positionCount; i++)
                {
                    float percent = (float)i / (m_TargetDisplay.positionCount - 1);
                    m_TargetDisplay.SetPosition(i, Vector2.Lerp(transform.position, Target.position, percent));
                }

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
