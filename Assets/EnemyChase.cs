using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
public sealed class EnemyChase : MonoBehaviour, IMovementProvider, IWeaponProvider
{
    public event Action<int, bool> FireEvent;

    public bool EnginesCut { get; private set; }
    public float Steering { get; private set; }

    private void Update()
    {
        
    }
}
