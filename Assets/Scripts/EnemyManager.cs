using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
public sealed class EnemyManager : MonoBehaviour
{
    const bool DrawGUI = true;

    List<EnemyInstance> m_TrackedEnemies;

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

    [Space]
    [SerializeField] private float m_WaterLevel;

    private float m_NextUpdateTime;
    private float m_GameTime;

#if UNITY_EDITOR
    private List<string> m_DebugLog;
#endif

    // Calculate the current pressure threshold, used to dictate when a new enemy should spawn. Increases over time.
    private float PressureThreshold => Mathf.Clamp(m_InitialPressureThreshold + Time.time * m_PressureOverTime, m_MinPressure, m_MaxPressure);

    [System.Serializable]
    private struct EnemyProfile
    {
        public GameObject prefab;
        public float weight;
        public float pressure;
        public float minPressureThreshold;
        public int deathScore;
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
        m_TrackedEnemies = new List<EnemyInstance>();

        m_NextUpdateTime = Time.time + m_InitialDelay;

#if UNITY_EDITOR
        m_DebugLog = new List<string>();
#endif
    }

    private void Update()
    {
        if (Time.time > m_NextUpdateTime)
        {
            // Calculate the current pressure and if it exeeds the threshold, spawn a random enemy.
            float currentPressure = EvaluateCurrentPressure();
            if (currentPressure < PressureThreshold)
            {
                SpawnRandomEnemyOnPlayer();
            }

            m_LastEvaluatedPressure = currentPressure;

            // Reset the timer.
            m_NextUpdateTime = Time.time + m_DelayBetweenUpdates;
        }

        // Increment gametime.
        m_GameTime += Time.deltaTime;
    }

    public float EvaluateCurrentPressure ()
    {
        // Calculate induvidual pressure components.
        float enemyPressure = m_EnemyPressureInfluence * GetEnemyPressure();
        float healthPressure = m_HealthPressureInfluence / GetPlayersHealth();

        // Calculate the total pressure, currently it increased with the enemies on screen based on type, and scaled with player health.
        float totalPressure = enemyPressure * healthPressure;

        // Smooth out the pressure to stop any frame spikes that would cause a disproportioned
        // amount of enemies to spawn.
        float finalPressure = Mathf.Lerp(totalPressure, m_LastEvaluatedPressure, m_PressureSmoothing);

#if UNITY_EDITOR
        // Write any debug info if in editor.
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
        // Find all players, sum and normalize their health.
        float totalHealth = 0f;
        foreach (PlayerController player in PlayerController.PlayerInstances)
        {
            totalHealth += player.CurrentHealth;
        }
        return totalHealth;
    }

    private float GetEnemyPressure()
    {
        // Remove any null instances in the tracked enemies incase any sneeked in.
        m_TrackedEnemies.RemoveAll(q => !q.instance.activeSelf);

        // Calculate the current pressure drived from enemies.
        float totalPressure = 0f;
        foreach (EnemyInstance instance in m_TrackedEnemies)
        {
            totalPressure += instance.pressure;
        }

        return totalPressure;
    }

    private EnemyInstance SpawnRandomEnemyOnPlayer ()
    {
        // Get a random enemy to spawn an enemy on.
        PlayerController targetPlayer = PlayerController.PlayerInstances[Random.Range(0, PlayerController.PlayerInstances.Count)];
        if (targetPlayer)
        {
            Vector2 randomVector = Random.insideUnitCircle;
            Vector2 position = (Vector2)targetPlayer.transform.position + randomVector.normalized * m_SpawnMinRange + randomVector * (m_SpawnMaxRange - m_SpawnMinRange);
            if (position.y > m_WaterLevel) return SpawnRandomEnemy(position);
        }

        return default;
    }

    private EnemyInstance SpawnRandomEnemy(Vector2 position)
    {
        // Calculate the current pool of enemies that can spawn and the total weight.
        List<EnemyProfile> enemyPool = new List<EnemyProfile>();
        float totalWeight = 0f;
        foreach (EnemyProfile profile in m_EnemyProfiles)
        {
            if (PressureThreshold > profile.minPressureThreshold)
            {
                enemyPool.Add(profile);
                totalWeight += profile.weight;
            }
        }

        // Evaluate the weighted pool to find a random enemy.
        float targetWeight = Random.Range(0f, totalWeight);
        float sumWeight = 0f;
        foreach (EnemyProfile profile in enemyPool)
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
        // Instance the enemy and add it to the tracking list.
        EnemyInstance newInstance = new EnemyInstance(Instantiate(profile.prefab, position, Quaternion.identity), profile.pressure);
        m_TrackedEnemies.Add(newInstance);

        if (newInstance.instance.TryGetComponent(out IDamagable damagable))
        {
            damagable.DeathEvent += (damage, damager, point, direction) =>
            {
                ScoreCounter.AddScoreStatic(profile.deathScore);
            };
        }

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

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (DrawGUI)
        {
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
            GUIStyle style = new GUIStyle { fontSize = 24 };
            GUI.Box(new Rect(0, 0, Screen.width, 128), debugLog, style);
            GUI.backgroundColor = new Color(0, 0, 0, 0);
        }
    }
#endif
}
