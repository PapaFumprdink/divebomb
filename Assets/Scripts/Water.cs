using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[ExecuteAlways]
public sealed class Water : MonoBehaviour
{
    [SerializeField] private float m_WaterLevel;
    [SerializeField] private ParticleSystem m_EnterEffect;
    [SerializeField] private Material m_WaterMaterial;
    [SerializeField] private AudioSource m_SplashSource;
    [SerializeField] private AudioClip m_InClip;
    [SerializeField] private AudioClip m_OutClip;

    private Camera m_MainCamera;

    private void Update()
    {
        if (!m_MainCamera) m_MainCamera = Camera.main;
        transform.position = new Vector3(m_MainCamera.transform.position.x, m_WaterLevel);

        if (m_WaterMaterial)
        {
            m_WaterMaterial.SetVector("_ReflectionPos", transform.position);
        }
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
    }
}
