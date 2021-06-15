using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
public sealed class Water : MonoBehaviour
{
    private Camera m_MainCamera;

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(m_MainCamera.transform.position.x, transform.position.y, 0f);
    }
}
