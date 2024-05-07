using NaughtyAttributes;
using UnityEngine;

public class BillboardCamera : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private Camera _mainCamera;

    void Update()
    {
        this.transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.back, _mainCamera.transform.rotation * Vector3.up);
    }
}
