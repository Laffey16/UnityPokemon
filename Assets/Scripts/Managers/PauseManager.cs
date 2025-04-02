using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button firstButton;
    [SerializeField] private DialogueRunner dialogueRunner;

    public bool isPaused;
    public static PauseManager Instance { get; private set; }

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


    public void TogglePause()
    {
        if (dialogueRunner.IsDialogueRunning) return;
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseMenu.SetActive(true);
            firstButton.Select();
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        Debug.Log($"Game is paused: {isPaused}");
    }

    #region Button Methods

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion
}