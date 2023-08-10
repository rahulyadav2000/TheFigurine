using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestIndicator : MonoBehaviour
{
    public Transform miniMapCam;
    public float miniMapRadius;
    private Vector3 questPos;
    void Update()
    {
        questPos = transform.parent.transform.position;
        questPos.y = transform.position.y;
        transform.position = questPos;

    }

    private void LateUpdate()
    {
        if( miniMapCam != null )
        {
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, miniMapCam.position.x - miniMapRadius, miniMapRadius + miniMapCam.position.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, miniMapCam.position.z - miniMapRadius, miniMapRadius + miniMapCam.position.z));
        }
        
    }
}
