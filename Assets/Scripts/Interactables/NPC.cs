using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private TextAsset dialogue;
    public int timesTalkedTo = 0;
    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogue.name);
    }
}