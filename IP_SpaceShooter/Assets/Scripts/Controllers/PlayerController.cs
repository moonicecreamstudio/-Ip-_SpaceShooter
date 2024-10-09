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
    private GameObject playerManagerObject;
    private PlayerManager playerManager;
    public int playerNumber;

    private Vector2 movementInput;

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

        controls.Gameplay.Shoot.performed += ctx => Shoot();

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (playerNumber == 1)
            {
                Debug.Log("Player 1 was destroyed!");
                playerManager.playerCount = -1;
                Destroy(gameObject);
            }
            if (playerNumber == 2)
            {
                Debug.Log("Player 2 was destroyed!");
                playerManager.playerCount = 0;
                Destroy(gameObject);
            }
        }
    }

    // Very basic controls.
    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void Shoot()
    {
        StartCoroutine(PlayHaptics(0.5f));
    }

    // Allows to play haptic feedback with the use of Couroutines.
    IEnumerator PlayHaptics(float seconds)
    {
        Gamepad.current.SetMotorSpeeds(0.25f, 0.25f);
        yield return new WaitForSeconds(seconds);
        InputSystem.ResetHaptics();
    }

}