using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButtons : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image pauseButtonImage;

    [SerializeField] private Sprite activatedSprite, deactivatedSprite;

    private Outline outline;

    // Core components of button
    private Button pauseButton;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        pauseButton = GetComponent<Button>();
        outline.enabled = false;

        pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        outline.enabled = false;
        pauseButtonImage.sprite = deactivatedSprite;
    }

    public void OnSelect(BaseEventData eventData)
    {
        outline.enabled = true;
        pauseButtonImage.sprite = activatedSprite;
    }

    private void OnPauseButtonClicked()
    {
        Debug.Log("Pause button clicked");
    }
}