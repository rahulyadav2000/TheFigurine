using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigurineCollector : MonoBehaviour
{
    


    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Player")
        {
            Destroy(gameObject);

        }
    }
}
