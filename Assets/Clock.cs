using System.Collections;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform secondHand;
    public Transform minuteHand;
    public Transform hourHand;

    IEnumerator Start()
    {
        float degreesPerSecond = 360 / 60;
        float degreesPerMinute = degreesPerSecond / 60;
        float degreesPerHour = degreesPerMinute / 12;
        while (true)
        {
            secondHand.Rotate(Vector3.forward, degreesPerSecond);
            minuteHand.Rotate(Vector3.forward, degreesPerMinute);
            hourHand.Rotate(Vector3.forward, degreesPerHour);
            yield return new WaitForSeconds(1);
        }
    }
}
