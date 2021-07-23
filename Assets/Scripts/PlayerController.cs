using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlayerController : MonoBehaviour, IMovementProvider, IWeaponProvider
{
    public static List<PlayerController> PlayerInstances = new List<PlayerController>();

    public event System.Action<int, bool, GameObject> FireEvent;
    public event System.Action CancelEvent;
    public event DamageAction DamageEvent;
    public event DamageAction DeathEvent;

    [SerializeField] private GameObject m_GameOverMenu;

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
    public bool EnginesCut => m_Controls.General.CutEngines.ReadValue<float>() > InputDeadzone && !Repairing;

    public bool Repairing => m_Controls.General.Repair.ReadValue<float>() > InputDeadzone;

    public float CurrentHealth => m_HealthComponent.CurrentHealth;
    public float MaxHealth => m_HealthComponent.MaxHealth;
    public float NormalizedHealth => m_HealthComponent.NormalizedHealth;
    public float InputDeadzone => 0.1f;

    private void Awake()
    {
        m_HealthComponent = GetComponent<IDamagable>();

        if (m_GameOverMenu) m_GameOverMenu.SetActive(false);
        m_HealthComponent.DeathEvent += (damage, damager, point, direction) => m_GameOverMenu.SetActive(true);

        // Store the main Camera for performance
        m_MainCamera = Camera.main;

        // Initalize the controls and subscribe to all the appropritate events.
        m_Controls = new Controls();

        m_Controls.General.Fire.performed += (ctx) =>
        {
            if (!Repairing)
            {
                EnemyBase closestEnemy = GetClosesEnemyToMouse();
                FireEvent?.Invoke((int)ctx.ReadValue<float>() - 1, true, closestEnemy ? closestEnemy.gameObject : null);
            }
        };

#if UNITY_EDITOR
        // Bind the debug controls only if in the unity editor.
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
        m_Controls.General.Cancel.performed += (ctx) => CancelEvent?.Invoke();
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
        if (m_Controls.General.Fire.ReadValue<float>() > InputDeadzone && !Repairing)
        {
            EnemyBase closestEnemy = GetClosesEnemyToMouse();
            FireEvent?.Invoke((int)m_Controls.General.Fire.ReadValue<float>() - 1, false, closestEnemy ? closestEnemy.gameObject : null);
        }
    }

    private EnemyBase GetClosesEnemyToMouse ()
    {
        // Calculate the world space mouse position.
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = m_MainCamera.ScreenToWorldPoint(mousePos);

        EnemyBase bestEnemy = null;

        // Loop through all of the instanced enemies and check which one is closest.
        foreach (EnemyBase enemy in EnemyBase.EnemyInstances)
        {
            if (!bestEnemy)
            {
                bestEnemy = enemy;
            }
            else 
            {
                float distanceToEnemy = ((Vector2)enemy.transform.position - worldPos).sqrMagnitude;
                float distanceToBestEnemy = ((Vector2)bestEnemy.transform.position - worldPos).sqrMagnitude;
                if (distanceToEnemy < distanceToBestEnemy)
                {
                    bestEnemy = enemy;
                }
            }
        }

        return bestEnemy;
    }
}
