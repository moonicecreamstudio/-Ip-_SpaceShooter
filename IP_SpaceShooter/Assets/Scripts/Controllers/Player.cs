using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerControls controls;

    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public GameObject powerupPrefab;
    public float movementSpeed = 0f;
    float maxMovementSpeed = 7.5f;
    float deacceleration = -3f;
    public float maxDetectionRange;
    public float radarLength;

    public int circlePoints;
    public int numberOfPowerups;
    public float radius;
    public int[] anglesList;
    public int[] powerupsAnglesList;
    public Vector3[] powerupsAnglesList2;
    public Vector3[] circleVectors;

    // Keely's Movement

    // Basic Character Movement: Velocity
    public float maxSpeed;
    private Vector3 currentVelocity;

    // Acceleration
    public float accelerationTime;
    private float acceleration;

    // Deceleration
    public float decelerationTime;
    private float deceleration;

    public bool isAccelerating = false;
    public bool isDecelerating;

    Vector2 move;

    private Vector2 movementInput;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        acceleration = maxSpeed / accelerationTime;
        deceleration = maxSpeed / decelerationTime;
    }

    void Update()
    {
        // PlayerMovement();

        // KeelyPlayerMovement();

        // ControllerMovement();

        transform.Translate(new Vector3(movementInput.x, movementInput.y, 0) * movementSpeed * Time.deltaTime);

        DetectAsteroids(maxDetectionRange, asteroidTransforms);
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnPowerups(radius, numberOfPowerups);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(PlayHaptics(2f));
        }


        //if (controls.Gameplay.Movement.triggered == false)
        //{
        //    if (movementSpeed >= 0)
        //    {
        //        movementSpeed += deacceleration * Time.deltaTime;
        //    }
        //    Vector2 m = new Vector2(move.x, move.y) * movementSpeed * Time.deltaTime;
        //    transform.Translate(m);
        //    Debug.Log("Decceleration.");
        //}

        // Why does the for loop stop everything after that?
        EnemyRadar(radius, circlePoints);
    }

    //public void ControllerMovement()
    //{
    //    Vector2 m = new Vector2(move.x, move.y) * Time.deltaTime;
    //    transform.Translate(m);
    //}

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void ControllerMovement()
    {
        if (move != Vector2.zero)
        {
            if (movementSpeed < maxMovementSpeed)
            {
                movementSpeed += acceleration * Time.deltaTime;
            }
        }
        else
        {
            if (movementSpeed >= 0.2)
            {
                movementSpeed += deacceleration * Time.deltaTime;
            }
        }
        Vector2 m = new Vector2(move.x, move.y) * movementSpeed * Time.deltaTime;
        transform.Translate(m);
        Debug.Log("Acceleration.");
    }

    //public void Accelerating(InputAction.CallbackContext ctx)
    //{
    //    isAccelerating = true;
    //    if (isAccelerating == true)
    //    {
    //        Debug.Log("Acceleration.");
    //    }
    //    isAccelerating = false;
    //}

        public void KeelyPlayerMovement()
    {
        Vector2 currentInput = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentInput += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentInput += Vector2.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentInput += Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentInput += Vector2.down;
        }

        if (currentInput.magnitude > 0)
        {
            // Our character is accelerating
            currentVelocity += acceleration * Time.deltaTime * (Vector3)currentInput.normalized;

            if (currentVelocity.magnitude > maxSpeed)
            {
                currentVelocity = currentVelocity.normalized * maxSpeed;
            }
        }
        else
        {
            // Our character is develerating
            // Check to solve the problem with the rotation, if our player starts moving the opposite direction
            Vector3 velocityDelta = (Vector3)currentVelocity.normalized * deceleration * Time.deltaTime;
            if (velocityDelta.magnitude > currentVelocity.magnitude)
            {
                currentVelocity = Vector3.zero;
            }
            else
            {
                currentVelocity -= velocityDelta;
            }
        }
        transform.position += currentVelocity * Time.deltaTime;
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
        anglesList = new int[circlePoints + 1];

        for (int i = 0; i < circlePoints; i++)
        {
            anglesList[i] = i * (360 / circlePoints);
            float startPointX = Mathf.Cos(anglesList[i] * Mathf.Deg2Rad);
            float startPointY = Mathf.Sin(anglesList[i] * Mathf.Deg2Rad);
            Vector3 startingPoint = new Vector3(startPointX, startPointY) * radius + transform.position;

            anglesList[i + 1] = (i + 1) * (360 / circlePoints);
            float endPointX = Mathf.Cos(anglesList[i + 1] * Mathf.Deg2Rad);
            float endPointY = Mathf.Sin(anglesList[i + 1] * Mathf.Deg2Rad);
            Vector3 endingPoint = new Vector3(endPointX, endPointY) * radius + transform.position;

            Debug.DrawLine(startingPoint, endingPoint, Color.red);
        }
    }

    public void SpawnPowerups(float radius, int numberOfPowerups)
    {
        powerupsAnglesList = new int[numberOfPowerups];

        for (int i = 0; i < numberOfPowerups; i++)
        {
            anglesList[i] = i * (360 / circlePoints);
            float startPointX = Mathf.Cos(anglesList[i] * Mathf.Deg2Rad);
            float startPointY = Mathf.Sin(anglesList[i] * Mathf.Deg2Rad);
            Vector3 startingPoint = new Vector3(startPointX, startPointY) * radius + transform.position;

            anglesList[i + 1] = (i + 1) * (360 / circlePoints);
            float endPointX = Mathf.Cos(anglesList[i + 1] * Mathf.Deg2Rad);
            float endPointY = Mathf.Sin(anglesList[i + 1] * Mathf.Deg2Rad);
            Vector3 endingPoint = new Vector3(endPointX, endPointY) * radius + transform.position;

            Instantiate(powerupPrefab, powerupsAnglesList2[i], Quaternion.identity);
        }

    }

    IEnumerator PlayHaptics(float seconds)
    {
        Gamepad.current.SetMotorSpeeds(0.25f, 0.25f);
        yield return new WaitForSeconds(seconds);
        InputSystem.ResetHaptics();
    }

}
