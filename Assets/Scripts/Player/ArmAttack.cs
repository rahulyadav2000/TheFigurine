using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAttack : MonoBehaviour
{
    [SerializeField] private float damagePoint = 20f;
    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wanderer"))
        {
            EnemyHealthSystem wandererHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();
            if (wandererHealth != null)
            {
                wandererHealth.ReduceHealth(damagePoint);
                Debug.Log("Wanderer Current Health: " + wandererHealth.GetHealth());
            }
        }
    }
}
