using System;
using System.Collections;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform secondHand;
    public Transform minuteHand;
    public Transform hourHand;

    void Start()
    {
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        DateTime time = DateTime.Now;
        while (true)
        {
            secondHand.localEulerAngles = new Vector3(0, 0, time.Second * 6);
            minuteHand.localEulerAngles = new Vector3(0, 0, time.Minute * 6);
            hourHand.localEulerAngles = new Vector3(0, 0, time.Hour * 30);
            yield return new WaitForSeconds(1);
            time = time.AddSeconds(1);
            Debug.Log($"time = {Time.time}");
        }
    }
}
