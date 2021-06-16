using System;
using UnityEditor;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class EnemyBase : MonoBehaviour, IWeaponProvider
{
    protected EnemyChaceDebugData m_DebugData;

    public GameObject CurrentTarget { get; set; }
    public float ForceFleeDuration { get; set; }

    public event Action<int, bool> FireEvent;

    public void Fire(int index, bool down) => FireEvent?.Invoke(index, down);

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
}
