using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour, IShootable
{
    private const float SkinWidth = 0.1f;

    [SerializeField] private float m_Damage;
    [SerializeField] private float m_MuzzleSpeed;
    [SerializeField] private LayerMask m_Layermask;
    [SerializeField] private float m_Lifetime;

    [Space]
    [SerializeField] private GameObject[] m_HitFX;

    protected Rigidbody2D m_Rigidbody;
    private float m_Age;

    public GameObject Shooter { get; set; }

    private void Awake()
    {
        // Get the objects rigidbody component and set its velocity to the muzzle speed.
        m_Rigidbody = GetComponent<Rigidbody2D>();

        m_Rigidbody.velocity = transform.up * m_MuzzleSpeed;

        // Disable the registered effects in case I forgot.
        foreach (GameObject fx in m_HitFX)
        {
            fx.SetActive(false);
        }
    }

    protected virtual void FixedUpdate()
    {
        // increment the age, if we have passed our lifetime, destroy the bullet.
        m_Age += Time.deltaTime;
        if (m_Age > m_Lifetime)
        {
            Decay();
        }
        else
        {
            // Otherwise, raycast forwards for collision detection
            float speed = m_Rigidbody.velocity.magnitude * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(m_Rigidbody.position, m_Rigidbody.velocity, speed + SkinWidth, m_Layermask);

            if (hit)
            {
                // If an object is hit that isnt the object that instanced this.
                if (hit.transform.gameObject != Shooter)
                {
                    // If the interface can be damaged, damage it.
                    if (hit.transform.TryGetComponent(out IDamagable damagable))
                    {
                        damagable.Damage(m_Damage, Shooter, hit.point, m_Rigidbody.velocity.normalized);
                    }

                    // Loop through each registered effects and activate them.
                    foreach (GameObject fx in m_HitFX)
                    {
                        fx.transform.parent = null;
                        fx.transform.position = hit.point;
                        fx.SetActive(true);
                    }

                    // Destroy the projectile if we hit something.
                    Destroy(gameObject);
                }
            }
        }

        transform.up = m_Rigidbody.velocity.normalized;
    }

    private void Decay()
    {
        Destroy(gameObject);
    }
}
