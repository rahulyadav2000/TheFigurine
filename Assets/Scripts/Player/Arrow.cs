using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Arrow : MonoBehaviour
{
    public Vector3 target;
    public float speed;

    public Rigidbody rb;
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        float arrowSpeed = speed* Time.deltaTime;
        rb.AddForce(transform.up * arrowSpeed, ForceMode.Impulse);
    }

    public void setTarget(Vector3 target)
    {
        this.target = target;
    }


    public void OnCollisionEnter(Collision collision)
    {

        //Debug.Log("Raycast hit: " + collision.gameObject.name);


        if (collision.gameObject.CompareTag("Wanderer"))
        {
            EnemyHealthSystem wandererHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();
            if (wandererHealth != null)
            {
                wandererHealth.ReduceHealth(20);
                Debug.Log("Wanderer Current Health: " + wandererHealth.GetHealth());
            }
        }

        if (collision.gameObject.CompareTag("Smasher"))
        {
            EnemyHealthSystem smasherHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();
            if(smasherHealth != null)
            {
                smasherHealth.ReduceHealth(50);
                Debug.Log("Smasher Current Health: " + smasherHealth.GetHealth());
            }
        }

        if (collision.gameObject.CompareTag("Head"))
        {
            EnemyHealthSystem wandererHealth = collision.gameObject.GetComponentInParent<EnemyHealthSystem>();

            wandererHealth.ReduceHealth(100);
            Debug.Log("Killed By HeadShot!! Enemy Current Health: " + wandererHealth.GetHealth());
        }
    }
}
