using UnityEngine;
using Cinemachine;


[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(CinemachineImpulseSource))]
public sealed class ImpulseOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        if (TryGetComponent(out CinemachineImpulseSource source))
        {
            source.GenerateImpulse();
        }
    }
}
