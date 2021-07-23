using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementProvider
{
    public bool EnginesCut { get; }
    public float Steering { get; }
}
