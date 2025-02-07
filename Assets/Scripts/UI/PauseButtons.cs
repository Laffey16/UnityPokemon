using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PauseButtons : MonoBehaviour, ISelectHandler,IDeselectHandler
{
    // Core components of button
    private Button pauseButton;
   [SerializeField] private Image pauseButtonImage;
    private Outline outline;

    [SerializeField] private Sprite activatedSprite, deactivatedSprite;

    private void Awake() 
    {
        outline = GetComponent<Outline>();
        pauseButton = GetComponent<Button>();
        outline.enabled = false;

        pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    private void OnPauseButtonClicked()
    {
        Debug.Log("Pause button clicked");
    }

    public void OnSelect(BaseEventData eventData)
    {
        outline.enabled = true;
        pauseButtonImage.sprite = activatedSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        outline.enabled = false;
        pauseButtonImage.sprite = deactivatedSprite;
    }
}
