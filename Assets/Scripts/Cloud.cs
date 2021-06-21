using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class Cloud : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_ImpactFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If something enters the cloud, display the collision fx.
        m_ImpactFX.transform.position = collision.transform.position;
        m_ImpactFX.transform.rotation = collision.attachedRigidbody
            ? Quaternion.LookRotation(Vector3.forward, collision.attachedRigidbody.velocity) * Quaternion.Euler(0f, 0f, 180f)
            : collision.transform.rotation * Quaternion.Euler(0f, 0f, 180f);
        m_ImpactFX.Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If something exits the cloud, display the collision fx.
        m_ImpactFX.transform.position = collision.transform.position;
        m_ImpactFX.transform.rotation = collision.attachedRigidbody
            ? Quaternion.LookRotation(Vector3.forward, collision.attachedRigidbody.velocity)
            : collision.transform.rotation * Quaternion.Euler(0f, 0f, 180f);

        m_ImpactFX.Play();
    }
}
