using UnityEditor;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
public sealed class EnemyChase : EnemyBase, IMovementProvider
{
    [Range(-1f, 1f)][SerializeField] private float m_AttackDot;
    [SerializeField] private float m_AttackRange;
    [SerializeField] private float m_FleeMax;
    [SerializeField] private float m_FleeMin;

    private Camera m_MainCamera;
    private bool m_IsFleeing;

    public bool EnginesCut { get; private set; }
    public float Steering { get; private set; }

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    private void Update()
    {
        if (!CurrentTarget)
        {
            CurrentTarget = PlayerController.PlayerInstances[Random.Range(0, PlayerController.PlayerInstances.Count)].gameObject;
        }
        else
        {
            Vector2 vectorToTarget = (CurrentTarget.transform.position - transform.position);
            float dotToTarget = Vector2.Dot(vectorToTarget.normalized, transform.up);

            if (vectorToTarget.sqrMagnitude > m_FleeMax * m_FleeMax)
            {
                m_IsFleeing = false;
                m_DebugData.state = "Chasing";
            }

            if (vectorToTarget.sqrMagnitude < m_FleeMin * m_FleeMin || ForceFleeDuration > 0f)
            {
                m_IsFleeing = true;
                m_DebugData.state = "Fleeing";
            }

            float crossToTarget = Vector3.Cross(vectorToTarget.normalized, transform.up).z;
            float crossFromTarget = Vector3.Cross(transform.up, vectorToTarget.normalized).z;

            Steering = m_IsFleeing ? crossFromTarget : crossToTarget;

            Vector2 viewportPosition = m_MainCamera.WorldToViewportPoint(transform.position);
            Bounds cameraBounds = new Bounds(new Vector3(0.5f, 0.5f, 0f), new Vector3(0.95f, 0.95f, 1f));
            bool isInCamera = cameraBounds.Contains(viewportPosition);

            if (dotToTarget > m_AttackDot && vectorToTarget.sqrMagnitude < m_AttackRange * m_AttackRange && isInCamera)
            {
                Fire(0, false);
                m_DebugData.state = "Firing";
            }

            m_DebugData.name = gameObject.name;
            m_DebugData.targetName = CurrentTarget.name;
            m_DebugData.distanceToTarget = vectorToTarget.magnitude;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        float fireRotation = Mathf.Acos(m_AttackDot) * Mathf.Rad2Deg;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, fireRotation) * transform.up * m_AttackRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -fireRotation) * transform.up * m_AttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_FleeMin);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_FleeMax);

        Gizmos.color = Color.white;

        m_DebugData.DrawDebug(transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float currentRotation = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
        Vector2 steerDirection = new Vector2(Mathf.Cos(currentRotation - Steering), Mathf.Sin(currentRotation - Steering)) * 15f;
        Gizmos.DrawRay(transform.position, steerDirection);

        Gizmos.color = Color.white;
    }
}
