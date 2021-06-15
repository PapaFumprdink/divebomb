using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class Cloud : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_ImpactFX;
    [SerializeField] private ParticleSystem m_TrailFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_ImpactFX.transform.position = collision.transform.position;
        m_ImpactFX.transform.rotation = Quaternion.Euler(0f, 0f, 180f) * collision.transform.rotation;
        m_ImpactFX.Play();

        m_TrailFX.Play();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        m_TrailFX.transform.position = collision.transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_ImpactFX.transform.position = collision.transform.position;
        m_ImpactFX.transform.rotation = collision.transform.rotation;
        m_ImpactFX.Play();

        m_TrailFX.Stop();
    }
}
