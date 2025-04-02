using System;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    public static DialogueManager Instance { get; private set; }

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

    public event Action OnDialogueStart;


    public void StartDialogue(string dialogueName)
    {
        dialogueRunner.StartDialogue(dialogueName);
        OnDialogueStart?.Invoke();
    }
}