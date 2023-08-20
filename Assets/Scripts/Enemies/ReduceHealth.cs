using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceHealth: MonoBehaviour
{
    [SerializeField] private float damage;
    public GameObject particleObj;
    public bool isSmasherSwipe;
    public bool isSmasherSpecial;
    public bool isWandererAttack;
    public bool isWandererSpecialAttack;
    private void Start()
    {
        if(particleObj != null)
        {
            particleObj.SetActive(false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // reduces the player health, turn on the particle system and plays the sound for the particular enemy type
        {
            HealthSystem healthSystem = other.gameObject.GetComponent<HealthSystem>();
            healthSystem.ReduceHealth(damage);

            if (particleObj != null)
            {
                particleObj.SetActive(true);
            }
            Invoke(nameof(ParticleHandler),0.8f);
            Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[5]);

            if(isSmasherSpecial)
            {
                Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[8]);
            }
            
            if(isSmasherSwipe)
            {
                Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[11]);
            }
            
            if(isWandererAttack)
            {
                Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[10]);
            }
            
            if(isWandererSpecialAttack)
            {
                Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[9]);
            }
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
