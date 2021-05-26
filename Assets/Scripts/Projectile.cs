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
        m_Rigidbody = GetComponent<Rigidbody2D>();

        m_Rigidbody.velocity = transform.up * m_MuzzleSpeed;

        foreach (GameObject fx in m_HitFX)
        {
            fx.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        m_Age += Time.deltaTime;
        if (m_Age > m_Lifetime)
        {
            Decay();
        }
        else
        {
            float speed = m_Rigidbody.velocity.magnitude * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(m_Rigidbody.position, m_Rigidbody.velocity, speed + SkinWidth, m_Layermask);
            if (hit)
            {
                if (hit.transform.TryGetComponent(out IDamagable damagable))
                {
                    damagable.Damage(m_Damage, Shooter, hit.point, m_Rigidbody.velocity.normalized);
                }

                foreach (GameObject fx in m_HitFX)
                {
                    fx.transform.parent = null;
                    fx.transform.position = hit.point;
                    fx.SetActive(true);
                }
                Destroy(gameObject);
            }
        }
    }

    private void Decay()
    {
        Destroy(gameObject);
    }
}
