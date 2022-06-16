using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform objectTarget;
    public float followSpd = 10f;
    public float sensitivity = 100f;
    public float clampAngle = 70f;

    private float rotX;
    private float rotY;

    public Transform realCamera;
    public Vector3 dirNormalized;
    public Vector3 lastDir;
    public float minDistance;
    public float maxDistance;
    public float lastDistance;
    public float smoothness = 10f;
    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
        dirNormalized = realCamera.localPosition.normalized;
        lastDistance = realCamera.localPosition.magnitude;
    }

    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime);
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectTarget.position, followSpd * Time.deltaTime);
        lastDir = transform.TransformPoint(dirNormalized * maxDistance);
        RaycastHit hit;

        if(Physics.Linecast(transform.position, lastDir, out hit))
        {
            lastDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            lastDistance = maxDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * lastDistance, Time.deltaTime * smoothness);
    }
}
