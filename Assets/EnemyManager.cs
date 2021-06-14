using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
public sealed class EnemyManager : MonoBehaviour
{
    List<EnemyInstance> m_EnemyInstances;

    [SerializeField] private float m_DelayBetweenUpdates;
    [SerializeField] private float m_InitialDelay;
    [SerializeField] private float m_SpawnMaxRange;
    [SerializeField] private float m_SpawnMinRange;

    [Space]
    [SerializeField] private EnemyProfile[] m_EnemyProfiles;

    [Space]
    [SerializeField] private float m_LastEvaluatedPressure;

    [Space]
    [SerializeField] private float m_EnemyPressureInfluence;
    [SerializeField] private float m_HealthPressureInfluence;

    [Space]
    [SerializeField] private float m_InitialPressureThreshold;
    [SerializeField] private float m_PressureOverTime;
    [SerializeField] private float m_MaxPressure;
    [SerializeField] private float m_MinPressure;

    [Space]
    [Range(0f, 1f)][SerializeField] private float m_PressureSmoothing;

    private float m_NextUpdateTime;
    private float m_GameTime;

#if UNITY_EDITOR
    private List<string> m_DebugLog;
#endif

    private float PressureThreshold => Mathf.Clamp(m_InitialPressureThreshold + Time.time * m_PressureOverTime, m_MinPressure, m_MaxPressure);

    [System.Serializable]
    private struct EnemyProfile
    {
        public GameObject prefab;
        public float weight;
        public float pressure;
    }

    private struct EnemyInstance
    {
        public GameObject instance;
        public float pressure;

        public EnemyInstance(GameObject instance, float pressure)
        {
            this.instance = instance;
            this.pressure = pressure;
        }
    }

    private void Awake()
    {
        m_EnemyInstances = new List<EnemyInstance>();

        m_NextUpdateTime = Time.time + m_InitialDelay;

#if UNITY_EDITOR
        m_DebugLog = new List<string>();
#endif
    }

    private void Update()
    {
        if (Time.time > m_NextUpdateTime)
        {
            float currentPressure = EvaluateCurrentPressure();
            if (currentPressure < PressureThreshold)
            {
                SpawnRandomEnemyOnPlayer();
            }

            m_LastEvaluatedPressure = currentPressure;
            m_NextUpdateTime = Time.time + m_DelayBetweenUpdates;
        }

        m_GameTime += Time.deltaTime;
    }

    public float EvaluateCurrentPressure ()
    {
        float enemyPressure = m_EnemyPressureInfluence * GetEnemyPressure();
        float healthPressure = m_HealthPressureInfluence / GetPlayersHealth();

        float totalPressure = enemyPressure * healthPressure;
        float finalPressure = Mathf.Lerp(totalPressure, m_LastEvaluatedPressure, m_PressureSmoothing);

#if UNITY_EDITOR
        string debugText = $"Pressure Threshold ({PressureThreshold}) | Total Pressure ({totalPressure}) | Last Pressure ({m_LastEvaluatedPressure}) | Final Pressure {finalPressure} == Enemy Pressure ({enemyPressure}) * Inverse Health Pressure ({healthPressure})";
        m_DebugLog.Add(debugText);

        if (m_DebugLog.Count > 5)
        {
            m_DebugLog.RemoveAt(0);
        }
#endif

        return finalPressure;
    }

    private float GetPlayersHealth()
    {
        float totalHealth = 0f;
        foreach (PlayerController player in PlayerController.PlayerInstances)
        {
            totalHealth += player.CurrentHealth;
        }
        return totalHealth;
    }

    private float GetEnemyPressure()
    {
        m_EnemyInstances.RemoveAll(q => !q.instance.activeSelf);

        float totalPressure = 0f;
        foreach (EnemyInstance instance in m_EnemyInstances)
        {
            totalPressure += instance.pressure;
        }

        return totalPressure;
    }

    private EnemyInstance SpawnRandomEnemyOnPlayer ()
    {
        PlayerController targetPlayer = PlayerController.PlayerInstances[Random.Range(0, PlayerController.PlayerInstances.Count)];
        if (targetPlayer)
        {
            Vector2 randomVector = Random.insideUnitCircle;
            Vector2 position = (Vector2)targetPlayer.transform.position + randomVector.normalized * m_SpawnMinRange + randomVector * (m_SpawnMaxRange - m_SpawnMinRange);

            return SpawnRandomEnemy(position);
        }

        else return default;
    }

    private EnemyInstance SpawnRandomEnemy(Vector2 position)
    {
        float totalWeight = 0f;
        foreach (EnemyProfile profile in m_EnemyProfiles)
        {
            totalWeight += profile.weight;
        }

        float targetWeight = Random.Range(0f, totalWeight);
        float sumWeight = 0f;
        foreach (EnemyProfile profile in m_EnemyProfiles)
        {
            if (profile.weight + sumWeight > targetWeight)
            {
                return SpawnEnemy(profile, position);
            }
            sumWeight += profile.weight;
        }

        return default;
    }

    private EnemyInstance SpawnEnemy (EnemyProfile profile, Vector2 position)
    {
        EnemyInstance newInstance = new EnemyInstance(Instantiate(profile.prefab, position, Quaternion.identity), profile.pressure);
        m_EnemyInstances.Add(newInstance);

        return newInstance;
    }

    private void OnDrawGizmos()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.transform.position, m_SpawnMinRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.transform.position, m_SpawnMaxRange);

            Gizmos.color = Color.white;
        }
    }

    private void OnValidate()
    {
        m_SpawnMinRange = Mathf.Min(m_SpawnMinRange, m_SpawnMaxRange);
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        string debugLog = string.Empty;
        if (m_DebugLog != null)
        {
            foreach (string debugLine in m_DebugLog)
            {
                debugLog += $"{debugLine}\n";
            }
        }
        else
        {
            debugLog = "--Debug Log Not Initalized--";
        }
        GUI.Box(new Rect(0, 0, Screen.width, 128), debugLog);
        GUI.backgroundColor = new Color(0, 0, 0, 0);
#endif
    }
}
