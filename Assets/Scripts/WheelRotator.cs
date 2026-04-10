using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    [Header("Assign wheels")]
    public Transform frontLeft;
    public Transform frontRight;
    public Transform rearLeft;
    public Transform rearRight;

    [Header("Wheel settings")]
    public float wheelRadius = 0.4f; // подстрой если нужно
    public float rotationMultiplier = 1f; // тонкая настройка

    private float currentSpeed = 0f;

    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    void Update()
    {
        float angularSpeed = (currentSpeed / wheelRadius) * Mathf.Rad2Deg;
        float rotationAmount = angularSpeed * Time.deltaTime * rotationMultiplier;

        RotateWheel(frontLeft, rotationAmount);
        RotateWheel(frontRight, rotationAmount);
        RotateWheel(rearLeft, rotationAmount);
        RotateWheel(rearRight, rotationAmount);
    }

    private void RotateWheel(Transform wheel, float amount)
    {
        if (wheel == null) return;

        // вращение вокруг оси X
        wheel.Rotate(amount, 0f, 0f, Space.Self);
    }
}
