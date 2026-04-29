using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    [Header("Assign wheels")]
    public Transform frontLeft;
    public Transform frontRight;
    public Transform rearLeft;
    public Transform rearRight;

    [Header("Wheel settings")]
    public float frontWheelRadius = 0.25f;
    public float rearWheelRadius = 0.4f;
    public float rotationMultiplier = 0.0001f;

    private float currentSpeed = 0f;

    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    void Update()
    {
        RotateWheels(frontWheelRadius, new Transform[] { frontLeft, frontRight });
        RotateWheels(rearWheelRadius, new Transform[] { rearLeft, rearRight });
    }

    void RotateWheels(float wheelRadius, Transform[] wheels)
    {
        float angularSpeed = (currentSpeed / wheelRadius) * Mathf.Rad2Deg;
        float rotationAmount = angularSpeed * Time.deltaTime * rotationMultiplier;

        foreach (Transform wheel in wheels)
        {
            RotateWheel(wheel, rotationAmount);
        }
    }

    private void RotateWheel(Transform wheel, float amount)
    {
        if (wheel == null) return;

        wheel.Rotate(amount, 0f, 0f, Space.Self);
    }
}
