using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float time;

    public Timer(float time)
    {
        this.time = time;
    }

    public void Update()
    {
        time -= Time.deltaTime;
    }

    public float GetTimeLeft()
    {
        return time;
    }

    public string ToString()
    {
        int minutes = ((int) time) / 60;
        int seconds = ((int) time % 60);
        int milliseconds = ((int) (time * 1000f)) % 1000;
        return minutes + ":" + seconds + ":" + milliseconds;
    }
}
