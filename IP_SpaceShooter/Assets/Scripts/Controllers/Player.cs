﻿using System.Collections;
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
    public float maxDetectionRange;
    public float radarLength;

    void Update()
    {
        PlayerMovement();
        DetectAsteroids(maxDetectionRange, asteroidTransforms);
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
            if (movementSpeed >= 0)
            {
                movementSpeed += deacceleration * Time.deltaTime;
            }
                transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
        }

        
    }

    public void DetectAsteroids (float inMaxRange, List<Transform> inAsteroids)
    {
        foreach(Transform asteroid in inAsteroids)
        {
            float distanceToAsteroid = Vector3.Distance(asteroid.position, transform.position);
            if (distanceToAsteroid < inMaxRange)
            {
                // If we are in range of the current asteroid then we are supposed to draw a line here
                Vector3 startPoint = transform.position;
                Vector3 endPoint = (asteroid.position - transform.position).normalized * radarLength + transform.position;

                Debug.DrawLine(startPoint, endPoint, Color.green);
            }
        }
    }

}
