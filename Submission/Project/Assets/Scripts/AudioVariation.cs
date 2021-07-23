using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public sealed class AudioVariation : MonoBehaviour
{
    [SerializeField] private Vector2 m_PitchVariation;
    [SerializeField] private Vector2 m_DynamicsVariation;

    private AudioSource m_Source;

    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
    }

    public void Randomize ()
    {
        m_Source.pitch = Random.Range(m_PitchVariation.x, m_PitchVariation.y);
        m_Source.volume = Random.Range(m_DynamicsVariation.x, m_DynamicsVariation.y);
    }
}
