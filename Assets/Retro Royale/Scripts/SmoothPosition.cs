using UnityEngine;
using Mirror;
public class SmoothPosition : NetworkBehaviour
{
    private bool moving = false;
    //private Vector3 targetPosition;
    //public Quaternion targetRotation; //Optional of course
    private Vector3 targetPosition;
    public float smoothFactor = 2;
    void Update()
    {

        if(moving == true)
        {
            float moveY = smoothFactor * Time.deltaTime * 4f;
            this.gameObject.transform.Translate(0, moveY, 0);
            if(this.gameObject.transform.position.y > targetPosition.y)
            {
                moving = false;
            }
        }



        //if (moving == true)0
        //{
        //    //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothFactor);
        //    //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothFactor);
        //}
        //if(this.gameObject.transform.position.y >= targetPosition)
        //{
        //    moving = false;
        //}
    }

    //public void setJump(Vector3 v)
    //{
    //    this.targetPosition = v;
    //    moving = true;
    //}

    public void setJump(float f)
    {

        this.targetPosition = this.gameObject.transform.position;
        this.targetPosition.y += f; 
        moving = true;
    }

}
