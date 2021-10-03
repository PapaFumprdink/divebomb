using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public sealed class EnemyDisplayHUD : MonoBehaviour
{
    [SerializeField] private Canvas m_ParentCavas;
    [SerializeField] private RectTransform m_EnemyIndicator;
    [SerializeField] private float m_DistanceFromEdge;

    private List<RectTransform> m_IndicatorInstances;

    private void Awake()
    {
        m_IndicatorInstances = new List<RectTransform>();

        m_IndicatorInstances.Add(m_EnemyIndicator);
    }

    private void Update()
    {
        for (int i = 0; i < EnemyBase.EnemyInstances.Count - m_IndicatorInstances.Count; i++)
        {
            m_IndicatorInstances.Add(Instantiate(m_EnemyIndicator, m_EnemyIndicator.transform.parent));
        }

        for (int i = 0; i < m_IndicatorInstances.Count; i++)
        {
            if (i < EnemyBase.EnemyInstances.Count)
            {
                EnemyBase enemy = EnemyBase.EnemyInstances[i];

                if (enemy)
                {
                    if (enemy.isActiveAndEnabled)
                    {
                        Vector2 vectorFromCamera = Camera.main.WorldToViewportPoint(enemy.transform.position) * 2f - Vector3.one;
                        if (vectorFromCamera.x < -1 || vectorFromCamera.x > 1f || vectorFromCamera.y < -1 || vectorFromCamera.y > 1f)
                        {
                            Vector2 canvasSize = ((RectTransform)m_ParentCavas.transform).rect.size;
                            Vector2 clampedVector = vectorFromCamera / Util.MaxAbsolute(vectorFromCamera.x, vectorFromCamera.y);
                            Vector2 position = clampedVector * canvasSize * 0.5f - Vector2.ClampMagnitude(vectorFromCamera, 1f) * m_DistanceFromEdge;
                            m_IndicatorInstances[i].localPosition = position;
                            m_IndicatorInstances[i].gameObject.SetActive(true);
                            continue;
                        }
                    }
                }
            }

            m_IndicatorInstances[i].gameObject.SetActive(false);
        }
    }
}
