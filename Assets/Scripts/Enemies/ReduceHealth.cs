using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceHealth: MonoBehaviour
{
    [SerializeField] private float damage;
    public GameObject particleObj;
    private void Start()
    {
        if(particleObj != null)
        {
            particleObj.SetActive(false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.gameObject.GetComponent<HealthSystem>();
            healthSystem.ReduceHealth(damage);

            if (particleObj != null)
            {
                particleObj.SetActive(true);
            }
            Invoke(nameof(ParticleHandler),0.8f);
        }
    }

    public void ParticleHandler()
    {
        if (particleObj != null)
        {
            particleObj.SetActive(false);
        }
    }

}
