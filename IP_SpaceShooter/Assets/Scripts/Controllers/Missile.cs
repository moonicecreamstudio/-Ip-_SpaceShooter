using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class Missile : MonoBehaviour
{
    public float speed;
    Rigidbody2D missile;
    Vector3 missileCollisionPosition;
    public GameObject asteroid;
    float screenHeight = 11;
    float screenWidth = 19;
    // bool cannotHarmPlayer = true;
    // float timer;
    // float invulTime = 1.5f;
    bool affectedByGravity = false;
    public GameObject planet;
    public float gravitySpeed;

    // Start is called before the first frame update
    void Start()
    {
        missile = GetComponent<Rigidbody2D>();
        if (planet == null)
        {
            planet = GameObject.FindWithTag("Planet");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Disabling this because of issues with OnCollisionEnter2D
        // Missiles is pushing player away if not immediately destroyed

        //if (timer < invulTime)
        //{
        //    timer += Time.deltaTime;
        //}
        //if (timer >= invulTime)
        //{
        //    cannotHarmPlayer = false;
        //}

        Vector3 currentFacingDirection = transform.up;
        float facingAngle = Mathf.Atan2(currentFacingDirection.y, currentFacingDirection.x) * Mathf.Rad2Deg;

        missile.velocity = transform.up * (speed * Time.deltaTime);

        GravitationalPull();

        OutOfBound();
    }
    // When a missile hits an asteroid, find the angle from the point it hit,
    // Take the angle it's travelling, subtract it by the angle from the asteroid point and the bullet point,
    // 
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            float currentTravellingAngle = transform.rotation.z;
            Vector3 asteroidPosition = other.transform.position;
            missileCollisionPosition = transform.position;
            float angleOfAsteroidToCollisionPoint = Mathf.Atan2(asteroidPosition.y - missileCollisionPosition.y, asteroidPosition.x - missileCollisionPosition.x) * Mathf.Rad2Deg;
            float targetAngle = (angleOfAsteroidToCollisionPoint + 90) + currentTravellingAngle;


            transform.rotation = Quaternion.Euler(0,0, targetAngle);
        }

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("Hurt", SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }

    public void OutOfBound()
    {
        if (transform.position.y > screenHeight)
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -screenHeight)
        {
            Destroy(gameObject);
        }
        if (transform.position.x > screenWidth)
        {
            Destroy(gameObject);
        }
        if (transform.position.x < -screenWidth)
        {
            Destroy(gameObject);
        }
    }

    public void InsideGravity()
    {
        affectedByGravity = true;
    }
    public void OutsideGravity()
    {
        affectedByGravity = false;
    }
    public void GravitationalPull()
    {
        if (affectedByGravity == true)
        {
            float currentTravellingAngle = transform.rotation.z;
            Vector3 missileCurrentPositionInRadius = transform.position;
            float angleOfMissileToPlanet = Mathf.Atan2(missileCurrentPositionInRadius.y - planet.transform.position.y, missileCurrentPositionInRadius.x - planet.transform.position.x) * Mathf.Rad2Deg;
            float targetAngle = (angleOfMissileToPlanet + 90);

            float targetCurrentAngle = Mathf.DeltaAngle(currentTravellingAngle, targetAngle);
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetCurrentAngle, gravitySpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
