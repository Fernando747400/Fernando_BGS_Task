using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{

    private Camera _mainCamera;

    private void Update()
    {
        if(_mainCamera == null) _mainCamera = this.GetComponentInParent<EnemyMovement>().Target.gameObject.GetComponentInChildren<Camera>();
        if(_mainCamera == null) return;
        this.transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.back, _mainCamera.transform.rotation * Vector3.up);
    }
}
