using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlayerController : MonoBehaviour, IWeaponProvider
{
    private const float Deadzone = 0.1f;

    public event System.Action<int, bool> FireEvent;

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
    public bool EnginesCut => m_Controls.General.CutEngines.ReadValue<float>() > Deadzone;

    private void Awake()
    {
        m_MainCamera = Camera.main;

        m_Controls = new Controls();

        m_Controls.General.FirePrimary.performed += (ctx) => FireEvent?.Invoke(0, true);
    }

    private void OnEnable()
    {
        m_Controls.Enable();
    }

    private void OnDisable()
    {
        m_Controls.Disable();
    }

    private void Update()
    {
        if (m_Controls.General.FirePrimary.ReadValue<float>() > Deadzone) FireEvent?.Invoke(0, false);
    }
}
