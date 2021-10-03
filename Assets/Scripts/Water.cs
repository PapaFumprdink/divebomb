using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[ExecuteAlways]
public sealed class Water : MonoBehaviour
{
    [SerializeField] private float m_WaterLevel;
    [SerializeField] private float m_SurfaceWaterLevel;
    [SerializeField] private ParticleSystem m_EnterEffect;
    [SerializeField] private Material m_WaterMaterial;
    [SerializeField] private AudioSource m_SplashSource;
    [SerializeField] private AudioClip m_InClip;
    [SerializeField] private AudioClip m_OutClip;

    [Space]
    [SerializeField] private bool m_OverrideShaderImpacts;
    [SerializeField] private Vector4 m_OverrideImpactValue;

    private Camera m_MainCamera;
    [SerializeField] private List<Impact> m_Impacts = new List<Impact>();

    public static float WaterLevel { get; private set; }
    public static float SurfaceWaterLevel { get; private set; }

    private void Start()
    {
        WaterLevel = m_WaterLevel;
        SurfaceWaterLevel = m_SurfaceWaterLevel;
    }

    private void Update()
    {
        if (!m_MainCamera) m_MainCamera = Camera.main;
        transform.position = new Vector3(m_MainCamera.transform.position.x, m_WaterLevel);

        if (m_WaterMaterial)
        {
            m_WaterMaterial.SetVector("_ReflectionPos", transform.position);
        }

        Vector4[] impactsForShader = new Vector4[64];
        if (m_OverrideShaderImpacts)
        {
            impactsForShader[0] = m_OverrideImpactValue;
            for (int i = 1; i < impactsForShader.Length; i++)
            {
                impactsForShader[i] = new Vector4(0f, 0f, 0f, 0f);
            }

            m_WaterMaterial.SetVectorArray("_Impacts", impactsForShader);
        }
        else for (int i = 0; i < impactsForShader.Length; i++)
        {
            if (i < m_Impacts.Count)
            {
                Impact impact = m_Impacts[i];
                impactsForShader[i] = new Vector4(impact.position.x - transform.position.x, impact.position.y - transform.position.y, Time.time - impact.impactTime, impact.force);
            }
            else
            {
                impactsForShader[i] = new Vector4(0f, 0f, 0f, 0f);
            }
        }

        m_WaterMaterial.SetVectorArray("_Impacts", impactsForShader);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (IWaterInterator waterInterator in collision.GetComponents<IWaterInterator>())
        {
            waterInterator.EnterWater();
        }

        m_SplashSource.transform.position = collision.transform.position;
        m_SplashSource.PlayOneShot(m_InClip);

        m_EnterEffect.transform.position = collision.transform.position;
        m_EnterEffect.Play();

        m_Impacts.Add(new Impact(collision.transform.position, Time.time, collision.attachedRigidbody.velocity.magnitude * collision.attachedRigidbody.mass));
        while (m_Impacts.Count > 64)
        {
            m_Impacts.RemoveAt(0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (IWaterInterator waterInterator in collision.GetComponents<IWaterInterator>())
        {
            waterInterator.ExitWater();
        }

        m_SplashSource.transform.position = collision.transform.position;
        m_SplashSource.PlayOneShot(m_OutClip);

        m_EnterEffect.transform.position = collision.transform.position;
        m_EnterEffect.Play();

        m_Impacts.Add(new Impact(collision.transform.position, Time.time, collision.attachedRigidbody.velocity.magnitude * collision.attachedRigidbody.mass));
        while (m_Impacts.Count > 64)
        {
            m_Impacts.RemoveAt(0);
        }
    }

    [System.Serializable]
    public struct Impact
    {
        public Vector2 position;
        public float impactTime;
        public float force;

        public Impact(Vector2 position, float impactTime, float force)
        {
            this.position = position;
            this.impactTime = impactTime;
            this.force = force;
        }
    }
}
