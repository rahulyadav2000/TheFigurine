using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWitcher : MonoBehaviour
{
    private Transform player;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(player.position);

        StartCoroutine(ChangePosition());
    }

    public IEnumerator ChangePosition()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("spell", true);
        CharacterController characterController = player.GetComponent<CharacterController>();
        characterController.enabled = false;
        yield return new WaitForSeconds(2f);
        Vector3 randomPosition = new Vector3(Random.Range(20f, 220f), 0f, Random.Range(20f, 220f));
        player.position = randomPosition;
        characterController.enabled = true;
        Destroy(gameObject);
    }
}
