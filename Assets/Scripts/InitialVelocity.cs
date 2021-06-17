using UnityEditor.UIElements;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class InitialVelocity : MonoBehaviour
{
    [SerializeField] private Vector2 m_InitialVelocity;
    [SerializeField] private Vector2 m_LocalVelocity;
    [SerializeField] private Vector2 m_Variance;

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody2D rigidbody))
        {
            Vector2 velocity = m_InitialVelocity;
            velocity += (Vector2)transform.TransformDirection(m_LocalVelocity);
            velocity += new Vector2(Random.Range(-m_Variance.x, m_Variance.x), Random.Range(-m_Variance.y, m_Variance.y));
            rigidbody.velocity = velocity;
        }
    }
}
