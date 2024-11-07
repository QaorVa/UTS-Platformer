using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static int seconds = 0;
    public static int minutes = 0;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            seconds++;
            timer = 0f;
            /*Debug.Log("Seconds: " + seconds);*/

            if (seconds >= 60)
            {
                minutes++;
                seconds = 0;
                /*Debug.Log("Minutes: " + minutes);*/
            }
        }
    }
}
