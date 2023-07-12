using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealthTest : MonoBehaviour
{
    public HealthSystem healthSystem;

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            healthSystem.AddHealth(20);
            Debug.Log("Current Health: " + healthSystem.GetHealth());
        }
    }
}
