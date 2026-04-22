using UnityEngine;

public class CompassGauge : MonoBehaviour
{
    public Transform needle;

    public void SetHeading(float heading)
    {
        needle.localRotation = Quaternion.Euler(0, -heading, 0);
    }
}
