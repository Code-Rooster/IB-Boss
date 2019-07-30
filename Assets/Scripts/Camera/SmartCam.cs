using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCam : MonoBehaviour
{
    //public Room room;

    public Transform target;

    public float smoothTime = 0.3F;

    private Vector3 dampVec;
    private Vector3 velocity = Vector3.zero;

    public bool lockCam = true;

    void Update()
    {
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));

        dampVec = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if (lockCam)
        {
            //dampVec.x = Mathf.Clamp(dampVec.x, room.camLims[0].position.x, room.camLims[1].position.x);
            //dampVec.y = Mathf.Clamp(dampVec.y, room.camLims[0].position.y, room.camLims[1].position.y);
        }

        transform.position = dampVec;
    }
}
