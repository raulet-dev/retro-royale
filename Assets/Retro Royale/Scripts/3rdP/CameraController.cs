using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraController : NetworkBehaviour
{
    private Transform target;

    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float yawSpeed = 100f;
    public float pitch = 2f;

    private float currentZoom = 10f;
    private float currentYaw = 0f;
    private bool active = false;
    

    private void Update()
    {
        if (active)
        {
        currentZoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        }

    }

    void LateUpdate()
    {
        if (active)
        {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);
        transform.RotateAround(target.position, Vector3.up, currentYaw);
        }

    }

    public void setTarget(Transform o)
    {
        target = o;
    }

    public void setActive(bool b)
    {
        active = b;
    }

    public void cameraLoaded()
    {
        Debug.Log("Camera loaded");
    }
}
