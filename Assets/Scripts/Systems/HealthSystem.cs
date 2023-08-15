using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentHealth = GameData.health;
    }

    public void AddHealth(float health)
    {
        if(currentHealth != maxHealth )
        {
            currentHealth += health;
            GameData.health = currentHealth;
        }
    }

    public void ReduceHealth(float damagePoint)
    {
        if(currentHealth != 0)
        {
            currentHealth -= damagePoint;
            GameData.health = currentHealth;
        }
    }

    public float GetHealth()
    {
        currentHealth = GameData.health;
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
