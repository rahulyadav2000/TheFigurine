using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttack : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HealthSystem healthSystem = other.gameObject.GetComponent<HealthSystem>();
            healthSystem.ReduceHealth(damage);
        }
    }
}
