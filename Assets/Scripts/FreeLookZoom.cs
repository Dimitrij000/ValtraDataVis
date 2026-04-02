using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLookZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 30f;
    public float minRadius = 10f;
    public float maxRadius = 50f;

    private float _zoomInput;

    private CinemachineOrbitalFollow _cameraOrbitalFollower;

    public void Awake()
    {
        _cameraOrbitalFollower = GetComponent<CinemachineOrbitalFollow>();
    }

    // Input System callback
    public void OnZoom(InputValue value)
    {
        _zoomInput = value.Get<Vector2>().y;
    }

    private void Update()
    {
        if (Mathf.Abs(_zoomInput) > 0.01f)
        {
            AdjustRigRadius(_zoomInput * zoomSpeed * Time.deltaTime);
        }
    }

    private void AdjustRigRadius(float delta)
    {
        var radius = _cameraOrbitalFollower.Radius;
        radius = Mathf.Clamp(radius - delta, minRadius, maxRadius);
        _cameraOrbitalFollower.Radius = radius;
    }
}
