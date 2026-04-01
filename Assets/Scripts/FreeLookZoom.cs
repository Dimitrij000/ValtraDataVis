using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLookZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 2f;
    public float minRadius = 1f;
    public float maxRadius = 10f;

    private float _zoomInput;
    public CinemachineFreeLook _freeLookCamera;

    public void Awake()
    {
        _freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    // Input System callback
    public void OnZoom(InputAction.CallbackContext context)
    {
        _zoomInput = context.ReadValue<float>();
    }

    private void Update()
    {
        if (_freeLookCamera == null) return;

        if (Mathf.Abs(_zoomInput) > 0.01f)
        {
            AdjustRigRadius(_zoomInput * zoomSpeed * Time.deltaTime);
        }
    }

    private void AdjustRigRadius(float delta)
    {
        for (int i = 0; i < 3; i++)
        {
            var rig = _freeLookCamera.m_Orbits[i];
            rig.m_Radius = Mathf.Clamp(rig.m_Radius - delta, minRadius, maxRadius);
            _freeLookCamera.m_Orbits[i] = rig;
        }
    }
}
