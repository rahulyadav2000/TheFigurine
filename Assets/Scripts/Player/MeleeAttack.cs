using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float damage;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Smasher"))
        {
            Debug.Log("Working Collisionnnnn!");
            EnemyHealthSystem smasherHealth = other.gameObject.GetComponent<EnemyHealthSystem>();

            smasherHealth.ReduceHealth(damage - 80f);
            Debug.Log("Smasher Current Health: " + smasherHealth.GetHealth());
        }

        if(other.gameObject.CompareTag("Wanderer"))
        {
            EnemyHealthSystem wandererHealth = other.gameObject.GetComponent<EnemyHealthSystem>();

            wandererHealth.ReduceHealth(damage);
            Debug.Log("Wanderer Current Health: " + wandererHealth.GetHealth());

        }
    }



    

}