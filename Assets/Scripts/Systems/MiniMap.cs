using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    private void LateUpdate()
    {
        Vector3 newPos = player.position;

        newPos.y = transform.position.y;
        transform.position = newPos;

        // rotates the minimap camera based on the player rotation
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f); 
    }
}
