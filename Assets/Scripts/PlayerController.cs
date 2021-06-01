using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlayerController : MonoBehaviour, IWeaponProvider
{
    public static List<PlayerController> PlayerInstances = new List<PlayerController>();

    private const float Deadzone = 0.1f;

    public event System.Action<int, bool> FireEvent;

    private Controls m_Controls;
    private Camera m_MainCamera;

    public float Steering
    {
        get
        {
            // Get the direction to the mouse in world space.
            var direction = m_MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
            
            // Calculate the steering direction by taking the magnitude of the cross product.
            return Vector3.Cross(direction.normalized, transform.up).z;
        }
    }
    public bool EnginesCut => m_Controls.General.CutEngines.ReadValue<float>() > Deadzone;

    private void Awake()
    {
        // Store the main Camera for performance
        m_MainCamera = Camera.main;

        // Initalize the controls and subscribe to all the appropritate events.
        m_Controls = new Controls();

        m_Controls.General.FirePrimary.performed += (ctx) => FireEvent?.Invoke(0, true);
    }

    private void OnEnable()
    {
        // Enable the controls on enable.
        m_Controls.Enable();

        PlayerInstances.Add(this);
    }

    private void OnDisable()
    {
        // Disable the controls on disable.
        m_Controls.Disable();

        PlayerInstances.Remove(this);
    }

    private void Update()
    {
        // Invoke any input events that are held.
        if (m_Controls.General.FirePrimary.ReadValue<float>() > Deadzone) FireEvent?.Invoke(0, false);
    }
}
