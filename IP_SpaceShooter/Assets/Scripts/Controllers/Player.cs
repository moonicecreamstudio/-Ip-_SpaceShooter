using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;
    public float movementSpeed = 0f;
    float maxMovementSpeed = 5f;
    float acceleration = 2f;
    float deacceleration = -2f;

    void Update()
    {
        PlayerMovement();
    }
    public void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            // Increase acceleration.
            if (movementSpeed < maxMovementSpeed)
            {
                movementSpeed += acceleration * Time.deltaTime;
                transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
            }
            // Once the player has hit top speed, continue moving at top speed.
            if (movementSpeed >= maxMovementSpeed)
            {
                movementSpeed = maxMovementSpeed;
                transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
            }
        }
        else
        {
            // Begin to decelerate.
                movementSpeed += deacceleration * Time.deltaTime; 
                transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
        }

        
    }

}
