using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleTransitionEffect : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1.5f;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private AnimationCurve shakeCurve;
    [SerializeField] private float shakeIntensity = 0.2f;
    
    [Header("References")]
    [SerializeField] private Image overlayImage;
    [SerializeField] private RectTransform leftPanel;
    [SerializeField] private RectTransform rightPanel;
    [SerializeField] private Camera mainCamera;
    
    private Vector3 originalCameraPosition;
    private bool isTransitioning = false;

    private void Awake()
    {
        // Make sure components are assigned
        if (overlayImage == null)
            overlayImage = transform.Find("Overlay").GetComponent<Image>();
        
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        // Initialize panels if not assigned in inspector
        if (leftPanel == null || rightPanel == null)
            Debug.LogError("Transition panels not assigned!");
            
        // Set initial states
        overlayImage.gameObject.SetActive(false);
        leftPanel.gameObject.SetActive(false);
        rightPanel.gameObject.SetActive(false);
    }
    
    public void TriggerBattleTransition(System.Action onTransitionComplete = null)
    {
        if (!isTransitioning)
            StartCoroutine(PlayTransition(onTransitionComplete));
    }
    
    private IEnumerator PlayTransition(System.Action onComplete)
    {
        isTransitioning = true;
        originalCameraPosition = mainCamera.transform.position;
        
        // Step 1: Initial camera shake
        yield return StartCoroutine(ShakeCamera(0.5f));
        
        // Step 2: Flash effect
        yield return StartCoroutine(FlashScreen());
        
        // Step 3: Diagonal slice transition
        yield return StartCoroutine(SliceTransition());
        
        // Step 4: Fade to battle scene
        yield return StartCoroutine(FadeInBattleScene());
        
        // Complete the transition
        isTransitioning = false;
        onComplete?.Invoke();
    }
    
    private IEnumerator ShakeCamera(float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float strength = shakeCurve.Evaluate(elapsed / duration);
            mainCamera.transform.position = originalCameraPosition + (Vector3)Random.insideUnitCircle * shakeIntensity * strength;
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        mainCamera.transform.position = originalCameraPosition;
    }
    
    private IEnumerator FlashScreen()
    {
        overlayImage.gameObject.SetActive(true);
        overlayImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        
        // Flash in
        float elapsed = 0f;
        float flashDuration = 0.2f;
        
        while (elapsed < flashDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsed / flashDuration);
            overlayImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        overlayImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 1);
        yield return new WaitForSeconds(0.1f);
        
        // Flash out
        elapsed = 0f;
        while (elapsed < flashDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsed / flashDuration);
            overlayImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        overlayImage.gameObject.SetActive(false);
    }
    
    private IEnumerator SliceTransition()
    {
        // Initialize panels
        leftPanel.gameObject.SetActive(true);
        rightPanel.gameObject.SetActive(true);
        
        // Set initial positions (offscreen)
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        leftPanel.sizeDelta = new Vector2(screenWidth * 2, screenHeight * 2);
        rightPanel.sizeDelta = new Vector2(screenWidth * 2, screenHeight * 2);
        
        // Position the panels
        leftPanel.anchoredPosition = new Vector2(-screenWidth, 0);
        rightPanel.anchoredPosition = new Vector2(screenWidth, 0);
        
        // Animate panels sliding in
        float elapsed = 0f;
        float slideDuration = 0.4f;
        
        Vector2 leftTargetPos = new Vector2(0, 0);
        Vector2 rightTargetPos = new Vector2(0, 0);
        
        while (elapsed < slideDuration)
        {
            float t = elapsed / slideDuration;
            t = 1 - Mathf.Pow(1 - t, 3); // Ease out cubic
            
            leftPanel.anchoredPosition = Vector2.Lerp(new Vector2(-screenWidth, 0), leftTargetPos, t);
            rightPanel.anchoredPosition = Vector2.Lerp(new Vector2(screenWidth, 0), rightTargetPos, t);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        leftPanel.anchoredPosition = leftTargetPos;
        rightPanel.anchoredPosition = rightTargetPos;
        
        yield return new WaitForSeconds(0.2f);
    }
    
    private IEnumerator FadeInBattleScene()
    {
        // This part would transition to your battle scene
        // You could either load a new scene or activate your battle objects
        
        // Example fade to black
        overlayImage.gameObject.SetActive(true);
        overlayImage.color = new Color(0, 0, 0, 0);
        
        float elapsed = 0f;
        float fadeDuration = 0.5f;
        
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            overlayImage.color = new Color(0, 0, 0, alpha);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        overlayImage.color = Color.black;
        
        // At this point you would typically:
        // 1. Load your battle scene
        // 2. Set up the battle
        // 3. Then fade out the black overlay
        
        yield return new WaitForSeconds(0.5f);
        
        // Example fade out code (you'd run this after loading battle scene)
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            overlayImage.color = new Color(0, 0, 0, alpha);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        overlayImage.gameObject.SetActive(false);
        leftPanel.gameObject.SetActive(false);
        rightPanel.gameObject.SetActive(false);
    }
}