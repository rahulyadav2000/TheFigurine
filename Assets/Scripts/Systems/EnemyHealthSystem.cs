using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float currentHealth;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void AddHealth(float health)
    {
        if(currentHealth != maxHealth )
        {
            currentHealth += health;
        }
    }

    public void ReduceHealth(float damagePoint)
    {
        if(currentHealth != 0)
        {
            currentHealth -= damagePoint;
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
