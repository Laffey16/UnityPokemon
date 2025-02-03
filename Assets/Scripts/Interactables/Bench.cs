using UnityEngine;

public class Bench : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Sitting on bench");
    }
}