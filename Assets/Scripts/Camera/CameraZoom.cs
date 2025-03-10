using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 15f;
    private float targetZoom;
    private float zoomVelocity;
    [SerializeField] float zoomSmoothTime = 0.1f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        targetZoom = _camera.fieldOfView;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        float zoomInput = context.ReadValue<float>();
        targetZoom -= zoomInput * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    

    private void Update()
    {
        _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, targetZoom, ref zoomVelocity, zoomSmoothTime);
    }
}
