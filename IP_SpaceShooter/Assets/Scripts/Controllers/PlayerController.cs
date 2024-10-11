using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    private float movementSpeed = 5.0f;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;
    public GameObject[] spawnArray;
    private GameObject playerManagerObject;
    private PlayerManager playerManager;
    public int playerNumber;
    float screenHeight = 10;
    float screenWidth = 18;
    public GameObject missile;
    public GameObject child;
    float desiredAngle;
    float desiredAngleUp;


    private Vector2 movementInput;
    private Vector2 rotateInput;
    private float shootInput;
    private bool missileCooldown = false;

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
        spawnArray[0] = GameObject.FindWithTag("Spawn1");
        spawnArray[1] = GameObject.FindWithTag("Spawn2");
        spawnArray[2] = GameObject.FindWithTag("Spawn3");
        spawnArray[3] = GameObject.FindWithTag("Spawn4");

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
        }
        if (playerNumber == 2)
        {
            spriteRenderer.sprite = spriteArray[1];
        }
    }

    void Update()
    {
        transform.Translate(new Vector3(movementInput.x, movementInput.y, 0) * movementSpeed * Time.deltaTime);

        RotationControls();
        ShootingControls();
        OutOfBounds();
    }

    // Ccontrols.
    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnRotate(InputAction.CallbackContext ctx) => rotateInput = ctx.ReadValue<Vector2>();

    public void Shoot(InputAction.CallbackContext ctx) => shootInput = ctx.ReadValue<float>();

    // Allows to play haptic feedback with the use of Couroutines.
    IEnumerator PlayHaptics(float seconds)
    {
        Gamepad.current.SetMotorSpeeds(0.25f, 0.25f);
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
        if (shootInput == 1 && missileCooldown == false)
        {
            Instantiate(missile, child.transform.position + (child.transform.up * 1.5f), child.transform.rotation);
            missileCooldown = true;
        }
        if (shootInput == 0)
        {
            missileCooldown = false;
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

    public void Hurt()
    {
        if (playerNumber == 1)
        {
            //StartCoroutine(PlayHaptics(0.5f));
            Debug.Log("Player 1 was destroyed!");
            playerManager.playerCount = -1;
            Destroy(gameObject);
        }
        if (playerNumber == 2)
        {
            //StartCoroutine(PlayHaptics(0.5f));
            Debug.Log("Player 2 was destroyed!");
            playerManager.playerCount = 0;
            Destroy(gameObject);
        }
    }

}