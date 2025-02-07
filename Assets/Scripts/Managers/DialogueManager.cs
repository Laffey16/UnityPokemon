using System;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    public static DialogueManager Instance { get; private set; }
    public event Action OnDialogueStart;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void StartDialogue(string dialogueName)
    {
        dialogueRunner.StartDialogue(dialogueName);
        OnDialogueStart?.Invoke();
    }
}
