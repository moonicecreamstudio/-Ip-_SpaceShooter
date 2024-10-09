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
    private int playerNumber;

    private Vector2 movementInput;

    void Awake()
    {
        controls = new PlayerControls();
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
        playerManagerObject = GameObject.FindWithTag("PlayerManager");
        if (playerManager == null)
        {
            playerManager = playerManagerObject.GetComponent<PlayerManager>();
        }
        playerManager.SendMessage("PlayerJoined", SendMessageOptions.DontRequireReceiver);

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (playerManager.playerCount == 0)
        {
            playerNumber = 1;
        }
        if (playerManager.playerCount == 1)
        {
            playerNumber = 2;
        }

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
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void Shoot()
    {
        StartCoroutine(PlayHaptics(0.5f));
    }

    IEnumerator PlayHaptics(float seconds)
    {
        Gamepad.current.SetMotorSpeeds(0.25f, 0.25f);
        yield return new WaitForSeconds(seconds);
        InputSystem.ResetHaptics();
    }

}