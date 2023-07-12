using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceHealth: MonoBehaviour
{
    [SerializeField] float damage;
    private void Start()
    {
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystem healthSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
            healthSystem.ReduceHealth(damage);
        }
    }
}
