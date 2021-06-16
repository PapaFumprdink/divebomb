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

    private Rigidbody2D m_Rigidbody;
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

            ContactFilter2D filter = new ContactFilter2D { useTriggers = false, layerMask = m_Layermask};
            RaycastHit2D[] hits = new RaycastHit2D[1];
            Physics2D.Raycast(m_Rigidbody.position, m_Rigidbody.velocity, filter, hits, speed + SkinWidth);
            RaycastHit2D hit = hits[0];

            if (hit)
            {
                bool didHit = false;

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

                    didHit = true;
                }
            }
        }
    }

    private void Decay()
    {
        Destroy(gameObject);
    }
}
