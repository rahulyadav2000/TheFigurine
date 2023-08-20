using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float damage;

    public void OnTriggerEnter(Collider other)
    {
        // if knife collides with smasher, it reduce the smasher's health
        if (other.gameObject.CompareTag("Smasher"))
        {
            EnemyHealthSystem smasherHealth = other.gameObject.GetComponent<EnemyHealthSystem>();

            smasherHealth.ReduceHealth(damage - 80f);
        }

        // if knife collides with wanderer, it reduce the wanderer's health
        if(other.gameObject.CompareTag("Wanderer"))
        {
            EnemyHealthSystem wandererHealth = other.gameObject.GetComponent<EnemyHealthSystem>();

            wandererHealth.ReduceHealth(damage);
        }
    }



    

}