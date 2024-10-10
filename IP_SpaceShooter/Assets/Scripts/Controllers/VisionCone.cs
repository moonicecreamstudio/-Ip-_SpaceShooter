using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public float detectionRadius;
    public float detectionAngle;
    public Transform targetTransform;
    Vector3 visionVector;
    Vector3 visionVectorCenter;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 lookingDirection = transform.up;

        //// Switching from vector to angle in terms of the looking direction
        //float lookingAngle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg;

        //// Why are we dividing it by 2?
        //// Tilting the angle to the left and right to get the limits of the field of view for the vision cone
        //float leftAngle = lookingAngle + visionAngle / 2;
        //float rightAngle = lookingAngle - visionAngle / 2;

        //// Switching from angle to vector in terms of the limits of the field of view for the vision cone
        //Vector3 leftVector = new Vector3(Mathf.Cos(leftAngle * Mathf.Deg2Rad), Mathf.Sin(leftAngle * Mathf.Deg2Rad));
        //Vector3 rightVector = new Vector3(Mathf.Cos(rightAngle * Mathf.Deg2Rad), Mathf.Sin(rightAngle * Mathf.Deg2Rad));

        //// Is the object too far to be visible

        //bool targetIsCloseEnough = Vector3.Distance(transform.position, targetTransform.position) < sightDistance;

        //// Is the object in the field of view?
        //bool targetIsInFOV = lookingAngle < leftAngle && lookingAngle > rightAngle;

        //Color lineColour;
        //if (targetIsCloseEnough && targetIsInFOV)
        //{
        //    lineColour = Color.green;
        //}
        //else
        //{
        //    lineColour = Color.red;
        //}

        //Debug.DrawLine(transform.position, leftVector * sightDistance + transform.position, lineColour);
        //Debug.DrawLine(transform.position, rightVector * sightDistance + transform.position, lineColour);


        //visionVector.x = Mathf.Cos(visionAngle * Mathf.Deg2Rad);
        //visionVector.y = Mathf.Sin(visionAngle * Mathf.Deg2Rad);

        //Vector3 reverseVision = new Vector3 (visionVector.x, -visionVector.y);

        //Debug.DrawLine(transform.position, (Vector3.Normalize(visionVector) * sightDistance) + transform.position, Color.green);
        //Debug.DrawLine(transform.position, (Vector3.Normalize(reverseVision) * sightDistance) + transform.position, Color.green);

        // ----------------------------------------------------------------- //

        Vector3 lookingDirection = transform.up;
        //Switching from vector to angle in terms of the looking directoin
        float lookingAngle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg;

        //Tilting the angle to the left and right to get the limits of the field of view for the vision cone
        float leftAngle = lookingAngle + detectionAngle / 2;
        float rightAngle = lookingAngle - detectionAngle / 2;

        //Switching from angle to vector in terms of the limits of the field of view for the vision cone
        Vector3 leftVector = new Vector3(Mathf.Cos(leftAngle * Mathf.Deg2Rad), Mathf.Sin(leftAngle * Mathf.Deg2Rad));
        Vector3 rightVector = new Vector3(Mathf.Cos(rightAngle * Mathf.Deg2Rad), Mathf.Sin(rightAngle * Mathf.Deg2Rad));





        //Is the object too far to be visible
        bool targetIsCloseEnough = Vector3.Distance(transform.position, targetTransform.position) < detectionRadius;

        //Is the object in the field of view?
        bool targetIsInFOV = lookingAngle < leftAngle && lookingAngle > rightAngle;

        Color lineColour;
        if (targetIsCloseEnough && targetIsInFOV)
        {
            lineColour = Color.green;
        }
        else
        {
            lineColour = Color.red;
        }

        Debug.DrawLine(transform.position, leftVector * detectionRadius + transform.position, lineColour);
        Debug.DrawLine(transform.position, rightVector * detectionRadius + transform.position, lineColour);
    }
}
