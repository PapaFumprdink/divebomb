using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlayerController : MonoBehaviour
{
    [SerializeField] private bool m_UseGamepad;

    private Controls m_Controls;
    private Camera m_MainCamera;

    public float Steering
    {
        get
        {
            if (m_UseGamepad)
            {
                var direction = m_Controls.General.TargetDirection.ReadValue<Vector2>();
                return Vector3.Cross(direction.normalized, transform.up).z;
            }
            else
            {
                var direction = m_MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
                return Vector3.Cross(direction.normalized, transform.up).z;
            }
        }
    }

    private void Awake()
    {
        m_MainCamera = Camera.main;

        m_Controls = new Controls();
    }

    private void OnEnable()
    {
        m_Controls.Enable();
    }

    private void OnDisable()
    {
        m_Controls.Disable();
    }
}
