using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothness;
    public Transform targetObject;
    private Vector3 initalOffset;
    private Vector3 cameraPosition;

    void Start()
    {
        transform.position = targetObject.position + new Vector3(0, 0, -10);
        initalOffset = transform.position - targetObject.position;
    }

    void FixedUpdate()
    {
        cameraPosition = targetObject.position + initalOffset;
        transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothness * Time.fixedDeltaTime);
    }
}
