using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DayNightCycle : MonoBehaviour
{
    public float dayTimeDuration;
    public float nightTimeDuration;

    public bool isDay;
    public bool isNight;

    private bool eOD;
    private bool nextDay;


    // Update is called once per frame
    void Update()
    {
        // Check if the rotation angle indicates it's day or night.
        if (transform.eulerAngles.x>0 && transform.eulerAngles.x<180)
        {
            isNight = false;
            isDay = true;
        }

        else if (transform.eulerAngles.x>180 && transform.eulerAngles.x <360)
        {
            isNight = true;
            isDay = false;
        }

        // Rotate the directional light based on whether it's day or night.
        if (isDay)
            transform.Rotate(1 * dayTimeDuration * Time.deltaTime, 0, 0);

        if (isNight)
            transform.Rotate(1 * nightTimeDuration * Time.deltaTime, 0, 0);

        DayIncreament();        
    }

    void DayIncreament()
    {
        // Check if the rotation angle indicates the end of the day.
        if (transform.eulerAngles.x>270 && transform.eulerAngles.x < 280)
        {
            eOD = true;
        }

        else
        {
            nextDay = false;
            eOD = false;
        }

        // If it's the end of the day and the next day hasn't been assigned
        {
            nextDay = true;
        }
    }
}
