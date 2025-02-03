using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGridMovement : MonoBehaviour
{
    private const int TILESIZE = 1;

    [Header("Tile Movement")] [SerializeField]
    private int walkSpeed = 4;

    private Animator animator;
    private Vector2 initialPos;
    private InteractionHandler interactionHandler;
    private bool isMoving;
    private Vector2 moveDirection = Vector2.zero;
    private float percentMovedToNextTile;

    private PlayerInput playerInput;
    private Rigidbody2D rb;

    private void Start()
    {
        initialPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        interactionHandler = GetComponentInChildren<InteractionHandler>();
        animator = GetComponentInChildren<Animator>();
        animator.speed = 0.5f;
    }

    private void Update()
    {
        if (!isMoving)
        {
            ProcessPlayerInput();
        }
        else
        {
            Move();
            Animation();
        }
    }

    private void Animation()
    {
        if (isMoving)
        {
            animator.SetFloat("x", moveDirection.x);
            animator.SetFloat("y", moveDirection.y);
        }
    }

    private void ProcessPlayerInput()
    {
        var rawInput = playerInput.actions["Move"].ReadValue<Vector2>();
        var sanitizedInput = GetSanitizedInput(rawInput);

        if (sanitizedInput != Vector2.zero)
        {
            moveDirection = sanitizedInput;
            isMoving = true;
            initialPos = transform.position;
            percentMovedToNextTile = 0f;
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private Vector2 GetSanitizedInput(Vector2 rawInput)
    {
        if (rawInput == Vector2.zero)
            return Vector2.zero;

        // Prioritize dominant axis and snap to cardinal directions
        var absX = Mathf.Abs(rawInput.x);
        var absY = Mathf.Abs(rawInput.y);

        if (absX > absY) return new Vector2(Mathf.Sign(rawInput.x), 0);

        return new Vector2(0, Mathf.Sign(rawInput.y));
    }

    private void Move()
    {
        percentMovedToNextTile += walkSpeed * Time.deltaTime;

        if (percentMovedToNextTile >= 1.0f)
        {
            // Snap to final position
            rb.MovePosition(initialPos + TILESIZE * moveDirection);
            percentMovedToNextTile = 0f;
            isMoving = false;
        }
        else
        {
            // Smoothly move towards target position
            rb.MovePosition(initialPos + TILESIZE * moveDirection * percentMovedToNextTile);
        }

        interactionHandler.rayDirection = moveDirection;
    }
}