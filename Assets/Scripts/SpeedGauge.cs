using UnityEngine;

public class SpeedGauge : MonoBehaviour
{
    public Transform needle; // стрелка
    public float maxSpeed = 80f;
    public float maxRotation = 220f; // угол стрелки при max speed
    public float minRotation = 0f;   // угол стрелки при 0 speed

    public void SetSpeed(float speed)
    {
        float t = Mathf.Clamp01(speed / maxSpeed);
        float angle = Mathf.Lerp(minRotation, maxRotation, t);
        needle.localRotation = Quaternion.Euler(0, angle, 0);
        Debug.Log(speed);
    }
}
