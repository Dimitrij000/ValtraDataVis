using UnityEngine;

public class SpeedGauge : MonoBehaviour
{
    public Transform needle;
    public float maxSpeed = 80f;
    public float maxRotation = 210f;
    public float minRotation = 0f;

    public void SetSpeed(float speed)
    {
        float t = Mathf.Clamp01(speed / maxSpeed);
        float angle = Mathf.Lerp(minRotation, maxRotation, t);
        needle.localRotation = Quaternion.Euler(0, angle, 0);
    }
}
