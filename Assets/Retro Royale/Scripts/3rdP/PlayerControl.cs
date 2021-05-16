using UnityEngine;
using Mirror;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : NetworkBehaviour
{

    public LayerMask movementMask;
    Camera cam;
    public override void OnStartLocalPlayer()
    {
        //cam = gameObject.GetComponentInChildren<Camera>();
        cam = transform.GetChild(0).GetComponent<Camera>();
        if(Camera.main.enabled == true) Camera.main.enabled = false;
        cam.enabled = true;
        cam.transform.SetParent(transform);
        cam.GetComponent<CameraController>().setTarget(transform);
        cam.GetComponent<CameraController>().setActive(true);
        cam.GetComponent<CameraController>().cameraLoaded();
        
    }

    void Update()
    {
        // local controls



        // network controls
        if (GetComponent<NetworkIdentity>().hasAuthority)//make sure this is an object that we ae controlling
        {
          
            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, movementMask))
                {
                    //Debug.Log("Hit " + hit.collider.name + " " + hit.point);
                    GetComponent<PhysicsLink>().CmdMoveToPint(hit.point);
                }
            }
        }
    }
}
