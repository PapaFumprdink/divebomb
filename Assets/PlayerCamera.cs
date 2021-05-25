using UnityEngine;

[ExecuteAlways]
public sealed class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform m_VirtualCamera;

    private void Awake()
    {
        m_VirtualCamera.parent = null;
    }
}
