using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public GameObject powerupPrefab;
    public float movementSpeed = 0f;
    float maxMovementSpeed = 5f;
    float acceleration = 2f;
    float deacceleration = -2f;
    public float maxDetectionRange;
    public float radarLength;

    public int circlePoints;
    public int numberOfPowerups;
    public float radius;
    public int[] anglesList;
    public int[] powerupsAnglesList;
    public Vector3[] powerupsAnglesList2;
    public Vector3[] circleVectors;



    void Update()
    {
        EnemyRadar(radius, circlePoints);
        PlayerMovement();
        DetectAsteroids(maxDetectionRange, asteroidTransforms);
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnPowerups(radius, numberOfPowerups);
        }
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

    public void EnemyRadar(float radius, int circlePoints)
    {
        anglesList = new int[circlePoints];

        for (int i = 0; i < circlePoints; i++)
        {
            anglesList[i] = 360 / circlePoints;
            float endPointX = Mathf.Cos(anglesList[i] * Mathf.Deg2Rad);
            float endPointY = Mathf.Sin(anglesList[i] * Mathf.Deg2Rad);
            Vector3 endingPoint = new Vector3(endPointX, endPointY) * radius + transform.position;
            Debug.DrawLine(transform.position, endingPoint, Color.red);
        }
    }

    public void SpawnPowerups(float radius, int numberOfPowerups)
    {
        powerupsAnglesList = new int[numberOfPowerups];

        for (int i = 0; i < numberOfPowerups; i++)
        {
            powerupsAnglesList[i] = 360 / numberOfPowerups;
            float endPointX = Mathf.Cos(powerupsAnglesList[i] * Mathf.Deg2Rad);
            float endPointY = Mathf.Sin(powerupsAnglesList[i] * Mathf.Deg2Rad);
            powerupsAnglesList2[i] = new Vector3(endPointX, endPointY) * radius + transform.position;
            for (int j = 0; j < numberOfPowerups; j++)
            {
                Instantiate(powerupPrefab, powerupsAnglesList2[i], Quaternion.identity);
            }
        }

    }

}
