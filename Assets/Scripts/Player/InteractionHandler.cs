using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private LayerMask layerMaskInteract;
    public Vector2 rayDirection;

    /*
        //If RED then no interactable object is in sight,
        // if BLUE then colliding with something, then if GREEN then it is an interactable object
    */
    private Color rayColor = Color.red;
    private bool hasInteracted = false;

    private void Update()
    {
        //Purely for debugging reasons, This shows where the raycast went
        Debug.DrawRay(transform.position, rayDirection * rayLength, rayColor);
        var hit = Physics2D.Raycast(transform.position, rayDirection, rayLength, layerMaskInteract);

        if (hit)
        {
            rayColor = Color.blue;
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                rayColor = Color.green;

                if (hasInteracted)
                {
                    interactable?.Interact();
                    hasInteracted = false;
                }
            }
        }
        else
        {
            rayColor = Color.red;
        }
    }

    public void Interact()
    {
        hasInteracted = true;
    }
}
