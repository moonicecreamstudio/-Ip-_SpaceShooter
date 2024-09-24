using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed;
    float arrivalDistance = 1f;
    float movementSpeed = 0.5f;
    float maxFloatDistance = 10f;
    float minFloatDistance = 2f;
    public Vector3 randomPoint;
    bool pointFound = false;
    float asteroidToRandomPoint;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AsteroidMovement();
    }

    public void AsteroidMovement()
    {

        var step = movementSpeed * Time.deltaTime;
        // Randomly creates a point within a square radius from the transform position of the asteroid.
        if (pointFound == false)
        {
            randomPoint = new Vector3(transform.position.x + Random.Range(-maxFloatDistance, maxFloatDistance + 1),
                                      transform.position.y + Random.Range(-maxFloatDistance, maxFloatDistance + 1));
            asteroidToRandomPoint = Vector3.Distance(transform.position, randomPoint);
        }

        if (asteroidToRandomPoint < maxFloatDistance && asteroidToRandomPoint > minFloatDistance)
        {
            pointFound = true;
            if (transform.position != randomPoint)
            {
                transform.position = Vector3.MoveTowards(transform.position, randomPoint, step);
            }
            if (Vector3.Distance(transform.position, randomPoint) < arrivalDistance)
            {
                pointFound = false;
            }

        }
    }
}
