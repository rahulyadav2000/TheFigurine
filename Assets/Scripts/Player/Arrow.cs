using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Arrow : MonoBehaviour
{
    public Vector3 target;
    public float speed;

    public Rigidbody rb;
    public GameObject particle;
    private void Start()
    {
        particle.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        float arrowSpeed = speed* Time.deltaTime;
        rb.AddForce(transform.up * arrowSpeed, ForceMode.Impulse); // adds force to the arrow prefab
    }

    public void setTarget(Vector3 target)
    {
        this.target = target;
    }


    public void OnCollisionEnter(Collision collision)
    {
        // if arrow collides with wanderer, it reduce the wanderer's health and enables the particle system
        if (collision.gameObject.CompareTag("Wanderer"))
        {
            EnemyHealthSystem wandererHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();
            if (wandererHealth != null)
            {
                wandererHealth.ReduceHealth(20);
                Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[0]);
                particle.SetActive(true);
            }
        }

        // if arrow collides with bear, it reduce the bear's health and enables the particle system
        if (collision.gameObject.CompareTag("Bear"))
        {
            EnemyHealthSystem bearHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();
            if(bearHealth != null)
            {
                bearHealth.ReduceHealth(50);
                Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[0]);
                particle.SetActive(true);

            }
        }

        // if arrow collides with smasher, it reduce the smasher's health and enables the particle system
        if (collision.gameObject.CompareTag("Smasher"))
        {
            EnemyHealthSystem smasherHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();
            if(smasherHealth != null)
            {
                smasherHealth.ReduceHealth(40);
                Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[0]);
                particle.SetActive(true);

            }
        }

        // if arrow collides with head of wanderer, it reduce the wanderer's health and enables the particle system
        if (collision.gameObject.CompareTag("Head"))
        {
            EnemyHealthSystem wandererHealth = collision.gameObject.GetComponentInParent<EnemyHealthSystem>();

            wandererHealth.ReduceHealth(100);
            Player.instance.source.PlayOneShot(AudioManager.instance.audioClip[0]);
            particle.SetActive(true);

        }
    }
}
