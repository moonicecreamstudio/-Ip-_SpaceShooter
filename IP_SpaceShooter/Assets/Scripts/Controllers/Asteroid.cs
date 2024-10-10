using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed;
    float arrivalDistance = 1f;
    float movementSpeed = 0.2f;
    float maxFloatDistance = 10f;
    float minFloatDistance = 2f;
    public Vector3 randomPoint;
    bool pointFound = false;
    float asteroidToRandomPoint;
    float screenHeight = 10;
    float screenWidgth = 18;

    void Update()
    {
        AsteroidMovement();
    }

    public void AsteroidMovement()
    {
        // Randomly creates a point within a square radius from the transform position of the asteroid.
        if (pointFound == false)
        {
            randomPoint = new Vector3(transform.position.x + Random.Range(-maxFloatDistance, maxFloatDistance + 1),
                                      transform.position.y + Random.Range(-maxFloatDistance, maxFloatDistance + 1));
            asteroidToRandomPoint = Vector3.Distance(transform.position, randomPoint);
        }

        if (asteroidToRandomPoint < maxFloatDistance && asteroidToRandomPoint > minFloatDistance &&
            randomPoint.x < screenWidgth && randomPoint.x > -screenWidgth &&
            randomPoint.y < screenHeight && randomPoint.y > -screenHeight)
        {
            pointFound = true;
            if (transform.position != randomPoint)
            {
                var step = movementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, randomPoint, step);
            }
            if (Vector3.Distance(transform.position, randomPoint) < arrivalDistance)
            {
                pointFound = false;
            }

        }
    }
}
