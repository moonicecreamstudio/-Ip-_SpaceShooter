using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Reflection;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    private float movementSpeed = 5.0f;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    public GameObject[] spawnArray;
    private GameObject playerManagerObject;
    private PlayerManager playerManager;
    private ScoreManager scoreManager;
    public int playerNumber;
    float screenHeight = 10;
    float screenWidth = 18;
    public GameObject missileRed;
    public GameObject missileBlue;
    public GameObject child;
    float desiredAngle;
    float desiredAngleUp;
    public GameObject blackHole;
    public int playerHealth = 3;
    public GameObject explosionRed;
    public GameObject explosionBlue;
    bool deathAnimation = false;
    bool disableControls = false;

    bool shootingButton = false;
    float deathTimer;

    private Vector2 movementInput;
    private Vector2 rotateInput;
    private float shootInput;
    private float shootingCooldownTimer;
    private bool shootingOK = true;

    private bool suction = false;
    bool explosionCheck = false;
    public float suctionSpeed;
    private float opacity;

    public GameObject triangle;

    void Awake()
    {
        controls = new PlayerControls();
    }

    // Stuff that needs to be used in order to have Unity's New Input System working.

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
        InputSystem.ResetHaptics();
        spawnArray[0] = GameObject.FindWithTag("Spawn1");
        spawnArray[1] = GameObject.FindWithTag("Spawn2");
        spawnArray[2] = GameObject.FindWithTag("Spawn3");
        spawnArray[3] = GameObject.FindWithTag("Spawn4");

        blackHole = GameObject.FindWithTag("BlackHoleRadius");

        transform.position = spawnArray[Random.Range(0,4)].transform.position;

        // By using the child to rotate the position, it won't affect the movement of the spaceship.

        child = this.gameObject.transform.GetChild(0).gameObject;

        // This section here is coded specifically for Game Objects that does not exist in the scene, but will be instantiated.
        // By using FindWithTag, this allows for instatiated Game Objects to find the scripts and other Game Objects required for stuff like SendMessage to work.

        playerManagerObject = GameObject.FindWithTag("PlayerManager");
        if (playerManager == null)
        {
            playerManager = playerManagerObject.GetComponent<PlayerManager>();
        }

        if (scoreManager == null)
        {
            scoreManager = playerManagerObject.GetComponent<ScoreManager>();
        }

        playerManager.SendMessage("PlayerJoined", SendMessageOptions.DontRequireReceiver);

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Player order.

        if (playerManager.playerCount == 0)
        {
            playerNumber = 1;
        }
        if (playerManager.playerCount == 1)
        {
            playerNumber = 2;
        }

        // This is here because in the case that both players die at the same time, or if Player 2 dies right after Player 1 dies.
        // It's a quick janky fix, that would need a better implementation if done properly.

        if (playerManager.playerCount == 2)
        {
            playerNumber = 1;
        }

        // Assigning the correct sprites to the player.

        if (playerNumber == 1)
        {
            spriteRenderer.sprite = spriteArray[0];
            triangle.GetComponent<Renderer>().material.color = new Color(0, 0, 1, 1);
            scoreManager.SendMessage("Player1ResetHealth", SendMessageOptions.DontRequireReceiver);
            scoreManager.SendMessage("ShowPlayer1Health", SendMessageOptions.DontRequireReceiver);
            scoreManager.SendMessage("HidePlayer1Start", SendMessageOptions.DontRequireReceiver);
        }
        if (playerNumber == 2)
        {
            spriteRenderer.sprite = spriteArray[1];
            triangle.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1);
            scoreManager.SendMessage("Player2ResetHealth", SendMessageOptions.DontRequireReceiver);
            scoreManager.SendMessage("ShowPlayer2Health", SendMessageOptions.DontRequireReceiver);
            scoreManager.SendMessage("HidePlayer2Start", SendMessageOptions.DontRequireReceiver);
        }
    }

    void Update()
    {
        if (disableControls == false)
        {
            transform.Translate(new Vector3(movementInput.x, movementInput.y, 0) * movementSpeed * Time.deltaTime);

            RotationControls();
            ShootingControls();
        }
        OutOfBounds();
        BlackHoleSuction();
        Dead();
        DeathAnimation();
    }

    // Ccontrols.
    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnRotate(InputAction.CallbackContext ctx) => rotateInput = ctx.ReadValue<Vector2>();

    public void Shoot(InputAction.CallbackContext ctx) => shootInput = ctx.ReadValue<float>();

    // Allows to play haptic feedback with the use of Couroutines.
    IEnumerator PlayHaptics(float seconds)
    {
        Gamepad.current.SetMotorSpeeds(0.5f, 0.5f);
        yield return new WaitForSeconds(seconds);
        InputSystem.ResetHaptics();
    }

    public void RotationControls()
    {
        // Creates a "deadzone". Maybe I could adjust that in the Unity system instead of doing it in here.
        if (rotateInput.x < 0.05 && rotateInput.x > -0.05 ||
            rotateInput.y < 0.05 && rotateInput.y > -0.05)
        {

        }
        else
        {
            desiredAngle = Mathf.Atan2(rotateInput.y, rotateInput.x) * Mathf.Rad2Deg;
        }

        desiredAngleUp = desiredAngle - 90;

        child.transform.rotation = Quaternion.Euler(0, 0, desiredAngleUp);
    }

    public void ShootingControls()
    {
        if (shootingCooldownTimer == 0)
        {
            shootingOK = true;
            shootingCooldownTimer += Time.deltaTime;
        }
        if (shootingOK == false)
        {
            shootingCooldownTimer += Time.deltaTime;
            if (shootingCooldownTimer > 0.5f)
            {
                shootingCooldownTimer = 0;
            }
        }
        if (shootInput == 1 && shootingButton == false && shootingOK == true)
        {
            if (playerNumber == 1)
            {
                Instantiate(missileBlue, child.transform.position + (child.transform.up * 1.5f), child.transform.rotation);
            }
            if (playerNumber == 2)
            {
                Instantiate(missileRed, child.transform.position + (child.transform.up * 1.5f), child.transform.rotation);
            }
            shootingButton = true;
            shootingOK = false;
        }
        if (shootInput == 0)
        {
            shootingButton = false;
        }
    }

    // When player is out of bounds, push them back
    public void OutOfBounds()
    {
        if (transform.position.y > screenHeight)
        {
            transform.position = new Vector3(transform.position.x, screenHeight - 0.1f);
        }
        if (transform.position.y < -screenHeight)
        {
            transform.position = new Vector3(transform.position.x, -screenHeight + 0.1f);
        }
        if (transform.position.x > screenWidth)
        {
            transform.position = new Vector3(screenWidth - 0.1f, transform.position.y);
        }
        if (transform.position.x < -screenWidth)
        {
            transform.position = new Vector3(-screenWidth + 0.1f, transform.position.y);
        }
    }

    public void BlackHoleSuction()
    {
        if (suction == true)
        {
            var step = suctionSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, blackHole.transform.position, step);
        }
    }
    public void CenterSucked()
    {
        if (playerNumber == 1)
        {
            playerHealth -= 3;
            scoreManager.SendMessage("Player1TakeDamage", 3, SendMessageOptions.DontRequireReceiver);
        }
        if (playerNumber == 2)
        {
            playerHealth -= 3;
            scoreManager.SendMessage("Player2TakeDamage", 3, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void Dead()
    {
        if (playerHealth <= 0)
        {
            if (playerNumber == 1)
            {
                Debug.Log("Player 1 was destroyed!");
                playerManager.playerCount = -1;
                if (explosionCheck == false)
                {
                    Instantiate(explosionBlue, transform.position, Quaternion.identity);
                    explosionCheck = true;
                }
                deathAnimation = true;
            }
            if (playerNumber == 2)
            {
                Debug.Log("Player 2 was destroyed!");
                playerManager.playerCount = 0;
                if (explosionCheck == false)
                {
                    Instantiate(explosionRed, transform.position, Quaternion.identity);
                    explosionCheck = true;
                }
                deathAnimation = true;
            }
        }
    }

    public void DeathAnimation()
    {
        if (deathAnimation == true)
        {
            disableControls = true;
            deathTimer = deathTimer + Time.deltaTime;

            opacity = opacity + Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f - opacity);

            if (playerNumber == 1)
            {
                triangle.GetComponent<Renderer>().material.color = new Color(0, 0, 1f, 1f - opacity);
            }
            if (playerNumber == 2)
            {
                triangle.GetComponent<Renderer>().material.color = new Color(1f, 0, 0, 1f - opacity);
            }

            if (deathTimer > 2)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Hurt()
    {
        if (playerNumber == 1)
        {
            StartCoroutine(PlayHaptics(0.25f));
            playerHealth -= 1;
            scoreManager.SendMessage("Player1TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
        }
        if (playerNumber == 2)
        {
            StartCoroutine(PlayHaptics(0.25f));
            playerHealth -= 1;
            scoreManager.SendMessage("Player2TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void InsideGravity()
    {
        suction = true;
    }
    public void OutsideGravity()
    {
        suction = false;
    }

}