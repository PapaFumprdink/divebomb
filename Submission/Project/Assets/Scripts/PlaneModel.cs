using UnityEngine;

[ExecuteAlways]
public sealed class PlaneModel : MonoBehaviour
{
    [SerializeField] private Transform m_ModelTransform;

    private void LateUpdate()
    {
        if (m_ModelTransform)
        {
            m_ModelTransform.localRotation = Quaternion.Euler(0f, -transform.eulerAngles.z, 0f);
        }
    }
}
