using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    private GameObject scene;
    private GameObject parent;
    private void Start()
    {
        scene = GameObject.Find("SceneScript");
    }
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision"); //this one occurs 1
        foreach (ContactPoint contact in collision.contacts)
        {
            if(contact.otherCollider.gameObject.CompareTag("Player"))
            {
                if(contact.otherCollider.gameObject == parent)
                {
                    break;
                } else
                {
                    //Debug.Log(this.name + "//" + contact.otherCollider.gameObject.name); //this block occurs 3 times
                    scene.GetComponent<SceneScript>().CmdKillPlayer(contact.otherCollider.gameObject, parent);
                    Destroy(this.gameObject);
                    break;
                }

            }
        }
    }

    public void setParent(GameObject o)
    {
        this.parent = o;
    }
}
