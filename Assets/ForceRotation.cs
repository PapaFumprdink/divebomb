using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
public sealed class ForceRotation : MonoBehaviour
{
    [SerializeField] private Vector3 m_Orientation;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(m_Orientation);
    }
}
