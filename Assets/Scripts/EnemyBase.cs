using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class EnemyBase : MonoBehaviour, IWeaponProvider
{
    public static List<EnemyBase> EnemyInstances { get; private set; } = new List<EnemyBase>();

    protected EnemyChaceDebugData m_DebugData;

    public GameObject CurrentTarget { get; set; }
    public float ForceFleeDuration { get; set; }

    public event Action<int, bool, GameObject> FireEvent;

    public void Fire(int index, bool down) => FireEvent?.Invoke(index, down, CurrentTarget);

    protected struct EnemyChaceDebugData
    {
        public string name;
        public string targetName;
        public float distanceToTarget;
        public string state;

        public void DrawDebug(Vector2 drawPosition)
        {
            string debugText = $"{name}\nTarget: {targetName}, {distanceToTarget}m away\nCurrently {state}";

#if UNITY_EDITOR
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.black;
            Handles.Label(drawPosition, debugText, style);
#endif
        }
    }

    protected virtual void OnEnable ()
    {
        EnemyInstances.Add(this);
    }

    protected virtual void OnDisable ()
    {
        EnemyInstances.Remove(this);
    }
}
