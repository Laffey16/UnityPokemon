using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerGridMovement PlayerGridMovement;
    private InteractionHandler interactionHandler;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        PlayerGridMovement = GetComponent<PlayerGridMovement>();
        interactionHandler = GetComponentInChildren<InteractionHandler>();

    }


    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) interactionHandler.Interact();
    }


    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) PauseManager.Instance.TogglePause();
    }
}