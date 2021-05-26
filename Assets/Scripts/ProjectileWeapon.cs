using UnityEngine;
using UnityEngine.Events;

public class ProjectileWeapon : Weapon
{
    [SerializeField] private GameObject m_ProjectilePrefab;
    [SerializeField] private Transform m_Muzzle;

    [Space]
    [SerializeField] private bool m_SingleFire;
    [SerializeField] private float m_RateOfFire;
    [SerializeField] private float m_Spray;
    [SerializeField] private int m_ProjectilesPerShot;

    [Space]
    [SerializeField] private UnityEvent m_FireEvent;

    private float m_NextFireTime;

    protected override void Fire(bool down)
    {
        if (m_SingleFire == down && Time.time > m_NextFireTime)
        {
            for (int i = 0; i < m_ProjectilesPerShot; i++)
            {
                Quaternion rotationOffset = Quaternion.Euler(0f, 0f, Random.Range(m_Spray / -2f, m_Spray / 2f));
                GameObject projectileInstance = Instantiate(m_ProjectilePrefab, m_Muzzle.position, m_Muzzle.rotation * rotationOffset);

                if (projectileInstance.TryGetComponent(out IShootable shootable))
                {
                    shootable.Shooter = gameObject;
                }
            }

            m_FireEvent?.Invoke();

            m_NextFireTime = Time.time + 60f / m_RateOfFire;
        }
    }
}
