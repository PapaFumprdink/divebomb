using UnityEngine;
using UnityEngine.Events;

public class ProjectileWeapon : Weapon
{
    [SerializeField] private GameObject m_ProjectilePrefab;
    [SerializeField] private Transform m_Muzzle;
    [SerializeField] private GameObject m_Shooter;

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
        // Check if enough time has passed since the last fire time and that the single fire condition lines up with whether the input was just pressed.
        if (m_SingleFire == down && Time.time > m_NextFireTime)
        {
            // Itterate for each projectile needed per shot.
            for (int i = 0; i < m_ProjectilesPerShot; i++)
            {
                // store the weapons random spray and instantiate the projectile at the muzzle.
                Quaternion rotationOffset = Quaternion.Euler(0f, 0f, Random.Range(m_Spray / -2f, m_Spray / 2f));
                GameObject projectileInstance = Instantiate(m_ProjectilePrefab, m_Muzzle.position, m_Muzzle.rotation * rotationOffset);

                // If the projectile has a shootable interface, set the shooter.
                if (projectileInstance.TryGetComponent(out IShootable shootable))
                {
                    shootable.Shooter = m_Shooter ? m_Shooter : gameObject;
                }
            }

            // Invoke the Fire event.
            m_FireEvent?.Invoke();

            // Set the next fire time to be the firerate's delay away.
            m_NextFireTime = Time.time + 60f / m_RateOfFire;
        }
    }
}
