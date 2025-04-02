using UnityEngine;
using UnityEngine.InputSystem;

public class TileBasedMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float tileSize = 0.5f;
    [SerializeField] private LayerMask collisionLayer; // Assign collision layer in inspector
    private Animator animator;
    private InteractionHandler interactionHandler;
    private bool isMoving;
    private Vector2 lastDirection = Vector2.up;
    private Vector2 movementInput;

    private PlayerInput playerInput;
    private Vector3 targetPosition;


    private void Start()
    {
        targetPosition = transform.position;
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        interactionHandler = GetComponentInChildren<InteractionHandler>();

        animator.SetFloat("x", 0);
        animator.SetFloat("y", 1);
        animator.SetBool("isMoving", false);
    }

    private void Update()
    {
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Convert input to single direction based on last pressed
        var currentDirection = Vector2.zero;
        if (movementInput.y > 0) currentDirection = Vector2.up;
        else if (movementInput.y < 0) currentDirection = Vector2.down;
        else if (movementInput.x > 0) currentDirection = Vector2.right;
        else if (movementInput.x < 0) currentDirection = Vector2.left;

        // Store the last non-zero direction
        if (currentDirection != Vector2.zero) lastDirection = currentDirection;

        // Update animator parameters
        animator.SetFloat("x", lastDirection.x);
        animator.SetFloat("y", lastDirection.y);
        animator.SetBool("isMoving", isMoving);

        // Handle movement
        if (currentDirection != Vector2.zero && !isMoving)
            if (CanMoveToDirection(currentDirection))
            {
                isMoving = true;
                targetPosition = transform.position + new Vector3(
                    currentDirection.x * tileSize,
                    currentDirection.y * tileSize,
                    0
                );
            }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false;

                if (currentDirection != Vector2.zero && CanMoveToDirection(currentDirection))
                {
                    targetPosition = transform.position + new Vector3(
                        currentDirection.x * tileSize,
                        currentDirection.y * tileSize,
                        0
                    );
                    isMoving = true;
                }
            }
        }

        interactionHandler.rayDirection = lastDirection;
    }

    private bool CanMoveToDirection(Vector2 direction)
    {
        // Cast a ray from current position to the next tile
        var hit = Physics2D.Raycast(
            transform.position,
            direction,
            tileSize,
            collisionLayer
        );

        // Return true if we didn't hit anything (can move)
        return hit.collider == null;
    }
}