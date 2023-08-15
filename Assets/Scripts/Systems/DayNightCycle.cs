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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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

        if (isDay)
            transform.Rotate(1 * dayTimeDuration * Time.deltaTime, 0, 0);

        if (isNight)
            transform.Rotate(1 * nightTimeDuration * Time.deltaTime, 0, 0);

        DayIncreament();        
    }

    void DayIncreament()
    {
        if(transform.eulerAngles.x>270 && transform.eulerAngles.x < 280)
        {
            eOD = true;
        }

        else
        {
            nextDay = false;
            eOD = false;
        }

        if(eOD && !nextDay)
        {
            nextDay = true;
        }
    }
}
