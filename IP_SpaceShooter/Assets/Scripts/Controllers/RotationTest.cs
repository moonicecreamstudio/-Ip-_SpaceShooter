using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public float angularSpeed;
    public float targetAngle;
    public Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.up, Color.green);
        Debug.DrawLine(transform.position, targetTransform.transform.position, Color.blue);

        Vector3 newTarget = targetTransform.transform.position - transform.position;

        targetAngle = (Mathf.Atan2(newTarget.y, newTarget.x) * Mathf.Rad2Deg - 90f);

        //targetAngle = (Mathf.Atan2(targetTransform.transform.position.x - transform.position.x,
        //                           targetTransform.transform.position.y - transform.position.y) * Mathf.Rad2Deg - 90f);

        // Debug.Log("targetAngle =" + targetAngle);

        // eulerAngles are the rotation of the object in degrees
        // We get access to them using transform
        // They are represented in the form of a Vector3 (same as position)
        if (transform.eulerAngles.z > targetAngle)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                                    transform.eulerAngles.y,
                                    targetAngle);
        }
        if (transform.eulerAngles.z < targetAngle)
        {
            transform.Rotate(0, 0, angularSpeed * Time.deltaTime);
        }


    }

    //This method will convert any provided angle so that it is between 
    //-180 and 180
    public float StandardizeAngle(float inAngle)
    {
        inAngle = inAngle % 360;

        inAngle = (inAngle + 360) % 360;

        if (inAngle > 180)
        {
            inAngle -= 360;
        }

        return inAngle;
    }

}
