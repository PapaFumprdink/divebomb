using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class CloudGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] m_CloudPrefabList;
    [SerializeField] private float m_CloudSpawnDistance;
    [SerializeField] private float m_CloudDropoffDistance;
    [SerializeField] private float m_CloudPopulation;
    [SerializeField] private Vector2 m_CloudScaleVariation;

    private List<GameObject> m_CloudInstances;
    private Camera m_MainCamera;

    private void Awake()
    {
        m_MainCamera = Camera.main;
        m_CloudInstances = new List<GameObject>();

        // Instance all of the clouds that will be used.
        for (int i = 0; i < m_CloudPopulation; i++)
        {
            // Get a random prefab from out list.
            GameObject prefab = m_CloudPrefabList[Random.Range(0, m_CloudPrefabList.Length)];
            Vector2 cloudPosition = (Vector2)m_MainCamera.transform.position + Random.insideUnitCircle * m_CloudSpawnDistance;

            // Spawn the new cloud and add it to our list.
            GameObject newCloud = Instantiate(prefab, cloudPosition, Quaternion.identity);
            newCloud.transform.localScale = Vector3.one * Random.Range(m_CloudScaleVariation.x, m_CloudScaleVariation.y);
            m_CloudInstances.Add(newCloud);
        }
    }

    private void Update()
    {
        foreach (GameObject cloud in m_CloudInstances)
        {
            // If one of our clouds is out of range from the camera, reset its position.
            if ((cloud.transform.position - m_MainCamera.transform.position).sqrMagnitude > m_CloudDropoffDistance * m_CloudDropoffDistance)
            {
                cloud.transform.position = (Vector2)m_MainCamera.transform.position + Random.insideUnitCircle.normalized * m_CloudSpawnDistance;
                cloud.transform.localScale = Vector3.one * Random.Range(m_CloudScaleVariation.x, m_CloudScaleVariation.y);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_CloudSpawnDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_CloudDropoffDistance);
        Gizmos.color = Color.white;
    }
}
