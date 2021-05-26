using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int m_WeaponIndex;
    
    protected IWeaponProvider m_Provider;

    private void Awake()
    {
        m_Provider = GetComponentInParent<IWeaponProvider>();
    }

    private void OnEnable()
    {
        m_Provider.FireEvent += OnFire;
    }

    private void OnDisable()
    {
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
