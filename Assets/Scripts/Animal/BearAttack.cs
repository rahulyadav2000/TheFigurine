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
            Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[5]);
            Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[7]);
        }
    }
}
