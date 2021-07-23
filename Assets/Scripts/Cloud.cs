using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class Cloud : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_ImpactFX;
    [SerializeField] private AnimationCurve m_ImpactFXScale;
    [SerializeField] private float m_SizeVariation;
    [SerializeField] private AnimationCurve m_ImpactFXSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShowImpactFX(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ShowImpactFX(collision, false);
    }

    private void ShowImpactFX(Collider2D collision, bool flip)
    {
        // If something exits the cloud, display the collision fx.
        Quaternion effectRotation = collision.attachedRigidbody
                    ? Quaternion.LookRotation(Vector3.forward, collision.attachedRigidbody.velocity) * Quaternion.Euler(0f, 0f, flip ? 180f : 0f)
                    : collision.transform.rotation * Quaternion.Euler(0f, 0f, 180f);

        ParticleSystem newFX = Instantiate(m_ImpactFX, collision.transform.position, effectRotation);
        if (collision.attachedRigidbody)
        {
            ParticleSystem.MainModule main = newFX.main;
            float particleScale = m_ImpactFXScale.Evaluate(collision.attachedRigidbody.velocity.magnitude);
            main.startSize = new ParticleSystem.MinMaxCurve
            {
                constantMin = particleScale - m_SizeVariation * particleScale,
                constantMax = particleScale + m_SizeVariation * particleScale,
                mode = ParticleSystemCurveMode.TwoConstants,
            };
            main.startSpeed = new ParticleSystem.MinMaxCurve
            {
                constantMin = 0,
                constantMax = m_ImpactFXSpeed.Evaluate(collision.attachedRigidbody.velocity.magnitude),
                mode = ParticleSystemCurveMode.TwoConstants,
            };
        }

        newFX.Play();
    }
}
