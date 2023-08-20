using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // if the player collides with the portal collider, it will destroy the game object and transition to the second scene
        if(other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(3);
            Destroy(gameObject, 0.5f);
        }
    }
}
