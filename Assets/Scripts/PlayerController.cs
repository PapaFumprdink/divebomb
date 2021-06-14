using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlayerController : MonoBehaviour, IMovementProvider, IWeaponProvider
{
    public static List<PlayerController> PlayerInstances = new List<PlayerController>();

    private const float Deadzone = 0.1f;

    public event System.Action<int, bool> FireEvent;
    public event DamageAction DamageEvent;
    public event DamageAction DeathEvent;

    private Controls m_Controls;
    private Camera m_MainCamera;
    private IDamagable m_HealthComponent;

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
    public bool EnginesCut => m_Controls.General.CutEngines.ReadValue<float>() > Deadzone && !Repairing;

    public bool Repairing => m_Controls.General.Repair.ReadValue<float>() > Deadzone;

    public float CurrentHealth => m_HealthComponent.CurrentHealth;
    public float MaxHealth => m_HealthComponent.MaxHealth;
    public float NormalizedHealth => m_HealthComponent.NormalizedHealth;

    private void Awake()
    {
        m_HealthComponent = GetComponent<IDamagable>();

        // Store the main Camera for performance
        m_MainCamera = Camera.main;

        // Initalize the controls and subscribe to all the appropritate events.
        m_Controls = new Controls();

        m_Controls.General.FirePrimary.performed += (ctx) =>
        {
            if (!Repairing)
            {
                FireEvent?.Invoke(0, true);
            }
        };

#if UNITY_EDITOR
        m_Controls.General.TimeSlowDebug.performed += (ctx) =>
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.002f;
        };

        m_Controls.General.TimeSlowDebug.canceled += (ctx) =>
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        };
#endif
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
        if (m_Controls.General.FirePrimary.ReadValue<float>() > Deadzone && !Repairing)
        {
            FireEvent?.Invoke(0, false);
        }
    }

    public void Damage(float damage, GameObject damager, Vector2 point, Vector2 direction)
    {
        throw new System.NotImplementedException();
    }

    public void Kill(float damage, GameObject killer, Vector2 point, Vector2 direction)
    {
        throw new System.NotImplementedException();
    }
}
