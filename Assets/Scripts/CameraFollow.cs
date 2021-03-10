using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private Vector3 cameraOffset = new Vector3(0, 0, -10);
    private float smoothSpeed = 0.05f;

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.transform.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
