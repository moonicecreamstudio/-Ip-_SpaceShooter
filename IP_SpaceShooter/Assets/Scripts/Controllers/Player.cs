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
    public float stoppingSpeed = 0f;
    float maxMovementSpeed = 10f;
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
                stoppingSpeed = 0f;
                movementSpeed += acceleration * Time.deltaTime;
                transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
                stoppingSpeed = movementSpeed;
            }
            // Once the player has hit top speed, continue moving at top speed.
            if (movementSpeed >= maxMovementSpeed)
            {
                stoppingSpeed = 0f;
                movementSpeed = maxMovementSpeed;
                transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
                stoppingSpeed = movementSpeed;
            }
        }
        else
        {
            // Begin to decelerate.
            if (stoppingSpeed > 0f)
            {
                movementSpeed = 0f;
                float lingeringHorizontalInput = horizontalInput;
                float lingeringVerticalInput = verticalInput;
                
                stoppingSpeed += deacceleration * Time.deltaTime;
                transform.position = transform.position + new Vector3(horizontalInput * stoppingSpeed * Time.deltaTime, verticalInput * stoppingSpeed * Time.deltaTime, 0);
            }
        }

        
    }

}
