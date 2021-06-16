using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int m_WeaponIndex;
    
    protected IWeaponProvider m_Provider;

    public abstract float Cooldown { get; }
    public abstract float NormalizedCooldown { get; }

    private void Awake()
    {
        // Get all required components
        m_Provider = GetComponentInParent<IWeaponProvider>();
    }

    private void OnEnable()
    {
        // Subscribe to all input events when the component is enabled
        m_Provider.FireEvent += OnFire;
    }

    private void OnDisable()
    {
        // Unsubscribe to all input events when the component is disabled
        m_Provider.FireEvent -= OnFire;
    }

    private void OnFire(int weaponIndex, bool down)
    {
        if (m_WeaponIndex == weaponIndex)
        {
            Fire(down);
        }
    }

    protected abstract void Fire(bool down);
}
