using System;
using UnityEngine;

public interface IWeaponProvider
{
    GameObject gameObject { get; }
    Transform transform { get; }

    event Action<int, bool> FireEvent;
}
