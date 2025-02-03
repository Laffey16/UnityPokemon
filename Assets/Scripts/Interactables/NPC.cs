using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private TextAsset dialogue;
    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue.name);
    }
}