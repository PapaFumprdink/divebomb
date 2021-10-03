using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
[SelectionBase]
[DisallowMultipleComponent]
public sealed class OutlineFX : MonoBehaviour
{
    public Renderer Renderer { get; private set; }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        OutlineManager.Register(this);
    }

    private void OnDisable()
    {
        OutlineManager.Unregister(this);
    }
}
