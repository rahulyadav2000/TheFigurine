using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestIndicator : MonoBehaviour
{
    public Transform miniMapCam;
    public float radius;
    private Vector3 questPos;
    void Update()
    {
        questPos = transform.parent.transform.position; // get the positon from the parent's transform 
        questPos.y = transform.position.y;  // keeps the y position
        transform.position = questPos;  // assigns the position 
    }

    private void LateUpdate()
    {
        if( miniMapCam != null )
        {
            // clamps the position within the radius of the minimap
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, miniMapCam.position.x - radius, radius + miniMapCam.position.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, miniMapCam.position.z - radius, radius + miniMapCam.position.z));
        }
        
    }
}
